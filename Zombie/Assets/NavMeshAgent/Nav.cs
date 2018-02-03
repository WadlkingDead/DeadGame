using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Nav : MonoBehaviour {
    private NavMeshAgent agent;
    private Camera cam;
    private Animator anim;
    public Transform targetTrans;
    public GameObject bullet;

    public static float targetDistance = 15;

    bool isBorn = false;

    private int moveSpeedID = Animator.StringToHash("MoveSpeed");
	// Use this for initialization
	void Start () 
    {
        agent = GetComponent<NavMeshAgent>();
        cam = GameObject.Find("Middle").GetComponent<Camera>();
        anim = GetComponent<Animator>();

        targetTrans = GameObject.FindWithTag("Player").transform;
	}
	
	// Update is called once per frame
	void Update ()
    {
        RaycastHit hit;
        if (Input.GetKeyDown(KeyCode.B))
        {
            /*Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
                SetDestination(hit.point);*/
            SelfBorn();
   
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayWalk();
            FindCar();
        }
        /* if (agent.enabled)
         {
             if (agent.remainingDistance != 0 && agent.remainingDistance <= 0.1f)
             {
                 agent.Stop();
                 agent.enabled = false;
                 anim.SetFloat(moveSpeedID, 0);
             }
             else
                 anim.SetFloat(moveSpeedID, agent.remainingDistance);
         }*/
        if (Input.GetKeyDown(KeyCode.T))
        {
            Attack();
        }


        if (!isBorn && targetTrans)
        {
            if (Vector3.Distance(transform.position, targetTrans.position) < targetDistance)
            {
                SelfBorn();
            }
        }
        

        if (isBorn)
        {
            //出生了  并且动画播放完毕
            AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);
            //播完动画 并且是idel 状态 
            if (info.normalizedTime >= 1.0f && info.IsName("on_all_four_B"))
            {
                PlayWalk();
                FindCar();
            }
         
        }

    }
    void SetDestination(Vector3 pos)
    {
        Debug.Log("SetDestination  =====   ");
        agent.enabled = true;                
        agent.SetDestination(pos);
        agent.angularSpeed = 35f;
        agent.speed = 0.6f;
    }

    public void Born()
    {
        if (Vector3.Distance(transform.position, targetTrans.position) < targetDistance)
        {
            Invoke("SelfBorn", Random.Range(0f, 2f));
        }
        
    }

    void SelfBorn()
    {
        anim.SetFloat(moveSpeedID, -2);
        isBorn = true;       
    }


    void FindCar()
    {
        SetDestination(targetTrans.position);
    }

    public void PlayIdel()
    {
        anim.SetFloat(moveSpeedID, 0);
    }
    public void PlayWalk()
    {
        anim.SetFloat(moveSpeedID, 0.5f);
    }
    public void PlayRun()
    {
        anim.SetFloat(moveSpeedID, 1f);
    }

    void Attack()
    {
        agent.enabled = false;
        anim.enabled = false;
        SetDestination(transform.position);

    }

}
