using UnityEngine;
using System.Collections;

public enum JWheelDrive
{
    Front = 0,
    Back = 1,
    All = 2
}


public class MotorControl : MonoBehaviour
{

    // if connected the controls will block if object not  active
    // (for example steer only if motor camera is active).

    public bool showNormalGizmos;
    public GameObject checkForActive;

    public Transform wheelFront; // connect to Front Right Wheel transform
    public Transform wheelBack; // connect to Front Left Wheel transform

    public Transform axleFront; // connect to Front Right Axle transform
    public Transform axlelBack; // connect to Front Left Axle transform

    public Transform bikeSteer;

    public Light[] brakeLights;
    public AudioClip brakeSound;
    public GameObject brakeParticle;
    public ParticleSystem shiftParticle, shiftParticle2;
    public AudioSource switchGear;
    public AudioSource nitro;

    private GameObject[] Particle = new GameObject[4];

    public float suspensionDistance = 0.2f; // amount of movement in suspension
    public float springs = 1000.0f; // suspension springs
    public float dampers = 2f; // how much damping the suspension has
    public bool AutomaticWheelRadius = true;
    public float wheelRadius = 0.25f; // the radius of the wheels
    public float torque = 100f; // the base power of the engine (per wheel, and before gears)
    public float shiftTorque = 100f;
    [HideInInspector]
    public float curTorque = 100f;
    public float brakeTorque = 2000f; // the power of the braks (per wheel)
    public float wheelWeight = 3f; // the weight of a wheel
    public Vector3 shiftSpeed = new Vector3(0.0f, -0.25f, 0.0f); // offset of centre of mass
    public Vector3 shiftCentre = new Vector3(0.0f, -0.25f, 0.0f); // offset of centre of mass

    public float maxSteerAngle = 30.0f; // max angle of steering wheels
    public float maxTurn = 1.0f;
    public JWheelDrive wheelDrive = JWheelDrive.Front; // which wheels are powered

    public float shiftDownRPM = 1500.0f; // rpm script will shift gear down
    public float shiftUpRPM = 2500.0f; // rpm script will shift gear up
    public float idleRPM = 500.0f; // idle rpm

    public float stiffness = 0.1f; // for wheels, determines slip
    public float swyStiffness = 0.1f; // for wheels, determines slip

    // gear ratios (index 0 is reverse)
    public float[] gears = { -10f, 9f, 6f, 4.5f, 3f, 2.5f };

    // automatic, if true motor shifts automatically up/down
    public bool automatic = true;

    // table of efficiency at certain RPM, in tableStep RPM increases, 1.0f is 100% efficient
    // at the given RPM, current table has 100% at around 2000RPM
    float[] efficiencyTable = { 0.6f, 0.65f, 0.7f, 0.75f, 0.8f, 0.85f, 0.9f, 1.0f, 1.0f, 0.95f, 0.80f, 0.70f, 0.60f, 0.5f, 0.45f, 0.40f, 0.36f, 0.33f, 0.30f, 0.20f, 0.10f, 0.05f };

    // the scale of the indices in table, so with 250f, 750RPM translates to efficiencyTable[3].
    float efficiencyTableStep = 250.0f;

    public Texture2D shiftGUI;

    private float powerShift = 100;
  

    private float shiftDelay = 0.0f;

    // shortcut to the component audiosource (engine sound).
    private AudioSource audioSource;



    [HideInInspector] public int currentGear = 1; // duh.
    [HideInInspector] public float motorRPM = 0.0f;
    [HideInInspector] public float speed;
    [HideInInspector] public float steer2;
    [HideInInspector] public bool groundHit;



    private float wantedRPM = 0.0f; // rpm the engine tries to reach
    private float curRotation;
    private float brackF = 0.0f;

    [HideInInspector]
    public bool crash;
    private bool shifmotor;

    float mRPM = 0;


    // every wheel has a wheeldata struct, contains useful wheel specific info
    class WheelData
    {
        public Transform transform;
        public GameObject go;
        public WheelCollider col;
        public Transform axle;
        public Vector3 startPos;
        public float rotation = 0.0f;
        public float maxSteer;
        public bool motor;
        public float curY = 0.0f;
    };

    WheelData[] wheels; // array with the wheel data

    // setup wheelcollider for given wheel data
    // wheel is the transform of the wheel
    // maxSteer is the angle in degrees the wheel can steer (0f for no steering)
    // motor if wheel is driven by engine or not
    WheelData SetWheelParams(Transform wheel, Transform axle, float maxSteer, bool motor, float curY)
    {
        if (wheel == null)
        {
            throw new System.Exception("wheel not connected to script!");
        }
        WheelData result = new WheelData(); // the container of wheel specific data

        // we create a new gameobject for the collider and move, transform it to match
        // the position of the wheel it represents. This allows us to do transforms
        // on the wheel itself without disturbing the collider.
        GameObject go = new GameObject("WheelCollider");
        go.transform.parent = transform; // the motor, not the wheel is parent
        go.transform.position = wheel.position; // match wheel pos
        go.transform.eulerAngles = transform.eulerAngles;
        curY = axle.localPosition.y;
        // create the actual wheel collider in the collider game object
        wheel.transform.gameObject.AddComponent<WheelCollider>();
        WheelCollider col = (WheelCollider)go.AddComponent(typeof(WheelCollider));
        col.transform.localScale = wheel.transform.localScale;
        col.radius = 0.33f;
        Destroy(wheel.transform.GetComponent<WheelCollider>());

        col.motorTorque = 0.0f;
        // store some useful references in the wheeldata object
        result.transform = wheel; // access to wheel transform 
        result.axle = axle;
        result.curY = curY;
        result.go = go; // store the collider game object
        result.col = col; // store the collider self
        result.startPos = axle.transform.localPosition; // store the current local pos of wheel
        result.maxSteer = maxSteer; // store the max steering angle allowed for wheel
        result.motor = motor; // store if wheel is connected to engine

        return result; // return the WheelData
    }

    // Use this for initialization
    Vector3 steerRot;
    Rigidbody rig;

    void Start()
    {

        // 4 wheels, if needed different size just modify and modify
        // the wheels[...] block below.
        wheels = new WheelData[2];
        steerRot = bikeSteer.eulerAngles;

        // setup wheels
        bool frontDrive = (wheelDrive == JWheelDrive.Front) || (wheelDrive == JWheelDrive.All);
        bool backDrive = (wheelDrive == JWheelDrive.Back) || (wheelDrive == JWheelDrive.All);

        // we use 4 wheels, but you can change that easily if neccesary.
        // this is the only place that refers directly to wheelFL, ...
        // so when adding wheels, you need to add the public transforms,
        // adjust the array size, and add the wheels initialisation here.
        wheels[0] = SetWheelParams(wheelFront, axleFront, maxSteerAngle, frontDrive, axleFront.localPosition.y);
        wheels[1] = SetWheelParams(wheelBack, axlelBack, 0.0f, backDrive, axlelBack.localPosition.y);


        // found out the hard way: some parameters must be set AFTER all wheel colliders
        // are created, like wheel mass, otherwise your motor will act funny and will
        // flip over all the time.
        foreach (WheelData w in wheels)
        {


            WheelCollider col = w.col;
            col.suspensionDistance = suspensionDistance;
            JointSpring js = col.suspensionSpring;
            js.spring = springs;
            js.damper = dampers;
            col.suspensionSpring = js;

            if (!AutomaticWheelRadius)
                col.radius = wheelRadius;

            col.mass = wheelWeight;

            // see docs, haven't really managed to get this work
            // like i would but just try out a fiddle with it.
            WheelFrictionCurve fc = col.forwardFriction;
            fc.asymptoteValue = 5000.0f;
            fc.extremumSlip = 2.0f;
            fc.asymptoteSlip = 20.0f;
            fc.stiffness = stiffness;
            col.forwardFriction = fc;
            fc = col.sidewaysFriction;
            fc.asymptoteValue = 7500.0f;
            fc.asymptoteSlip = 2.0f;
            fc.stiffness = swyStiffness;
            col.sidewaysFriction = fc;
        }

        // we move the centre of mass (somewhere below the centre works best.)


        // shortcut to audioSource should be engine sound, if null then no engine sound.
        //audioSource = (AudioSource)GetComponent(typeof(AudioSource));
        //if (audioSource == null)
        //{
        //    Debug.Log("No audio source, add one to the motor with looping engine noise (but can be turned off");
        //}
        rig = GetComponent<Rigidbody>();

        tempPos = transform.position;
    }




    void Update()
    {



        speed = rig.velocity.magnitude * 3.6f;


        if (Input.GetKeyDown("page up"))
        {
            ShiftUp();
        }
        if (Input.GetKeyDown("page down"))
        {
            ShiftDown();
        }

        
    }

    Vector3 tempPos;


    // handle shifting a gear up
    public void ShiftUp()
    {
        float now = Time.timeSinceLevelLoad;

        // check if we have waited long enough to shift
        if (now < shiftDelay) return;

        // check if we can shift up
        if (currentGear < gears.Length - 1)
        {
            if (!switchGear.isPlaying)
            {
                switchGear.GetComponent<AudioSource>().Play();
            }

            currentGear++;

            // we delay the next shift with 1s. (sorry, hardcoded)
            shiftDelay = now + 1.0f;
        }
    }


    // handle shifting a gear down
    public void ShiftDown()
    {
        float now = Time.timeSinceLevelLoad;

        // check if we have waited long enough to shift
        if (now < shiftDelay) return;

        // check if we can shift down (note gear 0 is reverse)
        if (currentGear > 0)
        {
            if (!switchGear.isPlaying)
            {
                switchGear.GetComponent<AudioSource>().Play();
            }

            currentGear--;

            // we delay the next shift with 1/10s. (sorry, hardcoded)
            shiftDelay = now + 0.1f;
        }
    }



	public float accel;
    
    void FixedUpdate()
    {
        mRPM = Vector3.Distance(transform.position, tempPos) * 1000;


        float delta = Time.fixedDeltaTime;
        rig.centerOfMass = shiftCentre;







        float steer = 0; // steering -1.0 .. 1.0
        //float accel = 0.0f; // accelerating -1.0 .. 1.0
        bool brake = false; // braking (true is brake)








        if (gameObject.activeSelf&&!crash)
        {
            if ((checkForActive == null) || checkForActive.gameObject.activeSelf)
            {
                // we only look at input when the object we monitor is
                // active (or we aren't monitoring an object).
                steer = Input.GetAxis("Horizontal");
                //accel = Input.GetAxis("Vertical");
                brake = Input.GetButton("Jump");
            }

        }







        foreach (Light brakeLight in brakeLights)
        {
            if (brake || accel < 0 || speed < 1.0f)
            {
                brakeLight.intensity = Mathf.Lerp(brakeLight.intensity, 8, 0.1f);
            }
            else
            {
                brakeLight.intensity = Mathf.Lerp(brakeLight.intensity, 0, 0.1f);
            }
        }




        steer2 = Mathf.LerpAngle(steer2, steer * maxSteerAngle, Time.deltaTime * 10);


        if (bikeSteer)
            bikeSteer.localEulerAngles = new Vector3(steerRot.x, Mathf.LerpAngle(bikeSteer.localEulerAngles.y, wheels[0].col.steerAngle, 0.2f), steerRot.z);



        Vector3 MotorRotation = transform.eulerAngles;

        MotorRotation.z = Mathf.LerpAngle(MotorRotation.z, -steer2 * maxTurn * (Mathf.Clamp(speed / 100, 0.0f, 1.0f)), 0.1f);


        if (!crash)
        {
            transform.eulerAngles = MotorRotation;

        }



        // handle automatic shifting
        if (automatic && (currentGear == 1) && (accel < 0.0f))
        {
            if (speed < 1.0f)
                ShiftDown(); // reverse


        }
        else if (automatic && (currentGear == 0) && (accel > 0.0f))
        {
            if (speed < 5.0f)
                ShiftUp(); // go from reverse to first gear

        }
        else if (automatic && (motorRPM > shiftUpRPM) && (accel > 0.0f) && !brake)
        {
            // if (speed > 20)
            ShiftUp(); // shift up

        }
        else if (automatic && (motorRPM < shiftDownRPM) && (currentGear > 1))
        {
            ShiftDown(); // shift down
        }




        if ((currentGear == 0))
        {
            shiftCentre.z = -accel / 5.0f;
            if (speed < gears[0] * -10)
                accel = -accel; // in automatic mode we need to hold arrow down for reverse
        }
        else
        {

            shiftCentre.z = -(accel / currentGear) / 3.0f;
        }







        // the RPM we try to achieve.
        wantedRPM = (5500.0f * accel) * 0.1f + wantedRPM * 0.9f;

        float rpm = 0.0f;
        int motorizedWheels = 0;
        bool floorContact = false;
        int currentWheel = 0;
        // calc rpm from current wheel speed and do some updating


        


        foreach (WheelData w in wheels)
        {
            WheelHit hit;
            WheelCollider col = w.col;



            // only calculate rpm on wheels that are connected to engine
            if (w.motor)
            {
                if (brake && currentGear == 1)
                {
                    rpm += accel * idleRPM;
                    if (rpm > 1)
                    {
                        shiftCentre.z = Mathf.PingPong(Time.time * (accel * 5), 0.5f) - 0.25f;
                    }
                    else
                    {
                        shiftCentre.z = 0.0f;
                    }
                }
                else
                {
                    rpm += col.rpm;
                }


                motorizedWheels++;
            }




            if (brake || accel < 0.0f)
            {
                if ((accel < 0.0f) || (brake && w == wheels[1]))
                {

                    if (brake && (accel > 0.0f))
                    {

                        brackF = Mathf.Lerp(brackF, 10, accel * Time.deltaTime);

                    }
                    else
                    {
                        brackF = Mathf.Lerp(brackF, 5, 0.2f * Time.deltaTime);
                    }

                    wantedRPM = 0.0f;
                    col.brakeTorque = brakeTorque;
                    w.rotation = curRotation;

                }
            }
            else
            {

                col.brakeTorque = accel == 0 ? col.brakeTorque = 200 : col.brakeTorque = 1;

                brackF = speed > 1 ?

                    (speed > 20 ? brackF = Mathf.Lerp(brackF, 1, 0.01f) : brackF = Mathf.Lerp(brackF, 3.0f, 0.01f))

                    : brackF = Mathf.Lerp(brackF, 0.0f, 0.01f);

                curRotation = w.rotation;
            }









            if (Input.GetKey(KeyCode.LeftShift) && speed > 5 && shifmotor&&steer==0)
            {

                if (powerShift == 0) { shifmotor = false; }

                powerShift = Mathf.MoveTowards(powerShift, 0.0f, 0.4f);

                nitro.volume = Mathf.Lerp(nitro.volume, 1.0f, 0.01f);

                if (!nitro.isPlaying)
                {
                    nitro.GetComponent<AudioSource>().Play();
                   
                }
                curTorque = powerShift > 0 ? shiftTorque : torque;
                shiftCentre.y = shiftSpeed.y;
                shiftCentre.z = shiftSpeed.z;
                shiftParticle.emissionRate = Mathf.Lerp(shiftParticle.emissionRate, powerShift > 0 ? 50 : 0, 0.4f);
                shiftParticle2.emissionRate = Mathf.Lerp(shiftParticle2.emissionRate, powerShift > 0 ? 50 : 0, 0.4f);
            }
            else
            {

                if (powerShift > 20)
                {
                    shifmotor = true;
                }

                nitro.volume = Mathf.Lerp(nitro.volume, 0.0f, 0.2f);
                powerShift = Mathf.MoveTowards(powerShift, 100.0f, 0.1f);
                curTorque = torque;
                shiftCentre.y = -0.6f;
                shiftParticle.emissionRate = Mathf.Lerp(shiftParticle.emissionRate, 0, 0.4f);
                shiftParticle2.emissionRate = Mathf.Lerp(shiftParticle2.emissionRate, 0, 0.4f);
            }












            // calculate the local rotation of the wheels from the delta time and rpm
            // then set the local rotation accordingly (also adjust for steering)

            w.rotation = Mathf.Repeat(w.rotation + delta * mRPM * 360.0f / 60.0f, 360.0f);
            // w.transform.localRotation = Quaternion.Euler(w.rotation, col.steerAngle, 0.0f);
            w.transform.localRotation = Quaternion.Euler(w.rotation, 0.0f, 0.0f);



            // let the wheels contact the ground, if no groundhit extend max suspension distance
            Vector3 lp = w.axle.localPosition;

            if (col.GetGroundHit(out hit))
            {


                if (brakeParticle)
                {
                    if (Particle[currentWheel] == null)
                    {

                        Particle[currentWheel] = Instantiate(brakeParticle, w.transform.position, Quaternion.identity) as GameObject;
                        Particle[currentWheel].name = "WheelParticle";
                        Particle[currentWheel].transform.parent = transform;
                        Particle[currentWheel].AddComponent<AudioSource>().clip = brakeSound;

                    }



                    var pc = Particle[currentWheel].GetComponent<ParticleEmitter>();

                    if (((brake || brackF > 3.0f || accel < 0.0f) && speed > 10) && currentGear > 0)
                    {
                        if ((accel < 0.0f) || ((brake || brackF > 3.0f) && (w == wheels[1])))
                        {

                            if (!Particle[currentWheel].GetComponent<AudioSource>().isPlaying)
                                Particle[currentWheel].GetComponent<AudioSource>().Play();
                            pc.emit = true;
                            Particle[currentWheel].GetComponent<AudioSource>().volume = Mathf.Clamp((speed / 25), 0, 1.0f);

                        }
                    }
                    else
                    {

                        pc.emit = false;
                        Particle[currentWheel].GetComponent<AudioSource>().volume = Mathf.Lerp(Particle[currentWheel].GetComponent<AudioSource>().volume, 0, 0.1f);
                    }

                }

                lp.y -= Vector3.Dot(w.transform.position - hit.point, Vector3.up / transform.lossyScale.x) - (col.radius);
                lp.y = Mathf.Clamp(lp.y, -10.0f, w.curY);

                floorContact = floorContact || (w.motor);

                if (!crash)
                {
                    rig.angularDrag = 10.0f;
                }
                else
                {
                    rig.angularDrag = 0.0f;


                }
                groundHit = true;

                rig.AddRelativeTorque(Vector3.forward, ForceMode.Force);
            }
            else
            {
                groundHit = false;

                rig.angularDrag = 0.0f;

                rig.AddForce(0, -1000, 0);
               

                if (Particle[3] != null)
                {
                    var pc = Particle[currentWheel].GetComponent<ParticleEmitter>();
                    pc.emit = false;
                }

                lp.y = w.startPos.y - suspensionDistance;
            }

            currentWheel++;
            w.axle.localPosition = lp;

        }





        // calculate the actual motor rpm from the wheels connected to the engine
        // note we haven't corrected for gear yet.
        if (motorizedWheels > 1)
        {
            rpm = rpm / motorizedWheels;
        }

        // we do some delay of the change (should take delta instead of just 95% of
        // previous rpm, and also adjust or gears.
        motorRPM = 0.95f * motorRPM + 0.05f * Mathf.Abs(rpm * gears[currentGear]);
        if (motorRPM > 5500.0f) motorRPM = 5200.0f;

        // calculate the 'efficiency' (low or high rpm have lower efficiency then the
        // ideal efficiency, say 2000RPM, see table
        int index = (int)(motorRPM / efficiencyTableStep);
        if (index >= efficiencyTable.Length) index = efficiencyTable.Length - 1;
        if (index < 0) index = 0;

        // calculate torque using gears and efficiency table
        float newTorque = curTorque * gears[currentGear] * efficiencyTable[index];

        // go set torque to the wheels
        foreach (WheelData w in wheels)
        {
            WheelCollider col = w.col;

            // of course, only the wheels connected to the engine can get engine torque
            if (w.motor)
            {
                // only set torque if wheel goes slower than the expected speed
                if (Mathf.Abs(col.rpm) > Mathf.Abs(wantedRPM))
                {
                    // wheel goes too fast, set torque to 0
                    col.motorTorque = 0;
                }
                else
                {
                    // 
                    float curTorqueCol = col.motorTorque;

                    if (!brake)
                        col.motorTorque = curTorqueCol * 0.9f + newTorque * 0.1f;


                }
            }



            // set steering angle
            if (brake)
            {
                col.steerAngle = Mathf.Lerp(col.steerAngle, steer * w.maxSteer, 0.1f);
            }
            else
            {

                float SteerAngle = Mathf.Clamp((speed / transform.lossyScale.x) / maxSteerAngle, 1.0f, maxSteerAngle);
                col.steerAngle = steer * (w.maxSteer / SteerAngle);
            }




        }



        // if we have an audiosource (motorsound) adjust pitch using rpm        
        //if (audioSource != null)
        //{
        //    // calculate pitch (keep it within reasonable bounds)
        //    float pitch = Mathf.Clamp(1.0f + ((motorRPM - idleRPM) / (shiftUpRPM - idleRPM)), 1.0f, 10.0f);
        //    audioSource.pitch = pitch;
        //    audioSource.volume = Mathf.MoveTowards(audioSource.volume, 1.0f, 0.02f);
        //}

        if (crash)
        {
            shiftCentre = Vector3.zero;
        }

        tempPos = transform.position;
    }







    /////////////// Show Normal Gizmos ////////////////////////////


    void OnDrawGizmos()
    {

        if (!showNormalGizmos || Application.isPlaying) return;

        Matrix4x4 rotationMatrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);

        Gizmos.matrix = rotationMatrix;
        Gizmos.color = new Color(1, 0, 0, 0.5f);

        Gizmos.DrawCube(Vector3.zero, new Vector3(0.5f, 1.0f, 2.5f));
        Gizmos.DrawSphere(shiftCentre / transform.lossyScale.x, 0.2f);

    }



    void OnGUI()
    {
        GUI.color = new Color(1,1,1,0.5f);

        GUI.DrawTexture(new Rect(Screen.width - 300, Screen.height - 25, 100, 20), shiftGUI);

        GUI.color = Color.green;

        GUI.DrawTexture(new Rect(Screen.width - 300, Screen.height - 25, powerShift, 20), shiftGUI);




    }



}