using UnityEngine;
using System.Collections;

public class MotorAnimation : MonoBehaviour
{

    public Transform player, playerRoot;

    public Transform eventPoint;
    public Transform hardcrash;
    public Animations animations;

    private MotorControl bikeScript;



    [System.Serializable]
    public class Animations
    {
        public AnimationClip stand, ride, right, left, back, jump;
    }


    private Vector3 myPosition;
    private Quaternion myRotation;
    private float timer;

    public float RestTime = 5;

    Animation playerAnim;

    void Start()
    {

        myPosition = player.localPosition;
        myRotation = player.localRotation;
        bikeScript = transform.GetComponent<MotorControl>();
        playerRoot.gameObject.SetActive(false);

        playerAnim = player.GetComponent<Animation>();
    }

    public bool canFall = false;

    void FixedUpdate()
    {



        Vector3 dir;


        timer = Mathf.MoveTowards(timer, 0.0f, 0.02f);

        if (transform.root.GetComponent<MotorControl>().groundHit)
        {

            dir = eventPoint.TransformDirection(Vector3.forward);

        }
        else
        {
            dir = eventPoint.TransformDirection(0, -0.25f, 1);
        }


        Debug.DrawRay(eventPoint.position, dir);

        RaycastHit hit;


        if (canFall && Physics.Raycast(eventPoint.position, dir, out hit, 1.0f) && GetComponent<MotorControl>().speed > 50)
        {
            if (player.parent != null)
            {


                //player.GetComponent<AudioSource>().Play();
                //hardcrash.GetComponent<AudioSource>().Play();
                player.parent = null;

            }

            transform.root.GetComponent<MotorControl>().crash = true;
            playerAnim.enabled = false;
            playerRoot.gameObject.SetActive(true);
            playerRoot.GetComponent<Rigidbody>().AddRelativeForce(0, 0, 5000);
            timer = RestTime;
        }



        if (timer == 0.0f)
        {

            playerAnim.enabled = true;
            playerRoot.gameObject.SetActive(false);

            player.parent = transform;
            player.localPosition = myPosition;
            player.localRotation = myRotation;

            transform.root.GetComponent<MotorControl>().crash = false;


        }





        if (playerRoot.gameObject.activeSelf) return;








        float steer = 0.0f;


        steer = Input.GetAxis("Horizontal");




        if (bikeScript.speed > 2)
        {


            if (bikeScript.currentGear != 0)
            {

                if (bikeScript.speed > 20)
                {

                    if (bikeScript.groundHit)
                    {
                        if (steer > 0)
                        {

                            playerAnim.CrossFade(animations.right.name, 0.5f);

                        }
                        else if (steer < 0)
                        {
                            playerAnim.CrossFade(animations.left.name, 0.5f);
                        }
                        else
                        {
                            playerAnim.CrossFade(animations.ride.name, 0.5f);
                        }

                    }
                    else
                    {
                        playerAnim.CrossFade(animations.jump.name, 0.5f);
                    }

                }
                else
                {
                    playerAnim.CrossFade(animations.ride.name, 0.5f);
                }

            }
            else
            {

                if (bikeScript.speed > 0.0f)
                {


                    playerAnim.CrossFade(animations.back.name, 0.5f);


                }



            }






        }
        else
        {

            playerAnim.CrossFade(animations.stand.name, 0.5f);

        }



    }






}
