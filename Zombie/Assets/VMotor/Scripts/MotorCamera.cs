using UnityEngine;
using System.Collections;

public class MotorCamera : MonoBehaviour
{

    public Transform target,player;
    public float smooth = 0.3f;
    public float distance = 5.0f;
    public float haight = 5.0f;
    public float Angle = 20;
    public Transform[] cameraSwitchView;
    public GUISkin GUISkin;

    private float yVelocity = 0.0f;
    private float xVelocity = 0.0f;
    private int Switch;
    private float backAngle = 0;





    void Update()
    {




        
        var carScript = (MotorControl)target.GetComponent<MotorControl>();
        GetComponent<Camera>().fieldOfView = Mathf.Clamp(carScript.speed / 20.0f + 60.0f, 60, 75);

        /* Import MotionBlur Effect
        if (transform.GetComponent<MotionBlur>())
        {
            if (carScript.curTorque == carScript.shiftTorque)
            {
                transform.GetComponent<MotionBlur>().blurAmount = Mathf.Lerp(transform.GetComponent<MotionBlur>().blurAmount, 0.5f, Time.deltaTime * 5);
            }
            else
            {
                transform.GetComponent<MotionBlur>().blurAmount = Mathf.Lerp(transform.GetComponent<MotionBlur>().blurAmount, 0.0f, Time.deltaTime * 5);
            }
        }
        */

        if (Input.GetKeyDown(KeyCode.C))
        {
            Switch++;
            if (Switch > cameraSwitchView.Length) { Switch = 0; }
        }



        if (Switch == 0)
        {
            // Damp angle from current y-angle towards target y-angle



            

            if (carScript.currentGear == 0 && carScript.speed > 2)
            {
                backAngle = 180;

            }
            else
            {
                backAngle = 0;
            }

            
            float yAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y,
            target.eulerAngles.y + backAngle, ref yVelocity, smooth);


            float xAngle = Mathf.SmoothDampAngle(transform.eulerAngles.x,
            target.eulerAngles.x + Angle, ref xVelocity, smooth);

            // Position at the target
            Vector3 position = target.position;
            // Then offset by distance behind the new angle
            position += Quaternion.Euler(Angle, yAngle, 0) * new Vector3(0, 0, -distance);
            // Apply the position
            //  transform.position = position;

            // Look at the target
            transform.eulerAngles = new Vector3(Angle, yAngle, 0);

            var direction = transform.rotation * -Vector3.forward;
            var targetDistance = AdjustLineOfSight(target.position + new Vector3(0, haight, 0), direction);


            transform.position = target.position + new Vector3(0, haight, 0) + direction * targetDistance;


        }
        else
        {

            transform.position = cameraSwitchView[Switch - 1].position;
            transform.rotation = Quaternion.Lerp(transform.rotation, cameraSwitchView[Switch - 1].rotation, Time.deltaTime * 10.0f);

        }


    }






    public LayerMask lineOfSightMask = 0;


    float AdjustLineOfSight(Vector3 target, Vector3 direction)
    {


        RaycastHit hit;

        if (Physics.Raycast(target, direction, out hit, distance, lineOfSightMask.value))
            return hit.distance;
        else
            return distance;

    }









//    public void OnGUI()
//    {
//
//
//        GUI.skin = GUISkin;
//
//
//
//
//        GUI.Box(new Rect(5, 5, 260, 250), "");
//
//        GUI.Label(new Rect(10, 10, 200, 50), "VMotor");
//
//        GUI.Label(new Rect(10, 50, 200, 50), "C key to change camera");
//
//        GUI.Label(new Rect(10, 80, 200, 50), "SPACE key to handbrake");
//
//        GUI.Label(new Rect(10, 100, 250, 50), "ARROWS keys or WASD keys to ride the motor");
//
//
//        GUI.Label(new Rect(10, 150, 200, 50), "R key to Rest Scene");
//
//        GUI.Label(new Rect(10, 180, 200, 50), "Holding shift at low speed could lift the wheels off the ground ");
//
//        if (Input.GetKeyDown(KeyCode.R))
//        {
//            Application.LoadLevel(Application.loadedLevel);
//        }
//
//
//
//    }




}
