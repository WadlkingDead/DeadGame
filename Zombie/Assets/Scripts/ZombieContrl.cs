using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieContrl : MonoBehaviour {

    Animator anim;

    static string moveSpeed = "MoveSpeed";
    static string damage = "Damage";
    static string death = "Death";
    static string wakeUp = "WakeUp";
    static string move = "Move";
    static string eat = "Eat";
    static string down = "Down";
    static string attack = "Attack";
    static string idle = "Idle2";


	// Use this for initialization
	void Awake () {
        anim = GetComponent<Animator>();
    }

    void OnEnable()
    {
        StartRandomState();
    }

    void StartRandomState()
    {
        int ran = Random.Range(0, 10);
        if (ran < 3)
        {
            anim.SetTrigger(eat);
            CallWakeUp(true);
        }
        else if (ran < 5)
        {
            BeginWakeUp();
        }
        else if (ran < 8)
        {
            CallWalk(false);
        }
        else
        {
            anim.SetTrigger(idle);
            CallWalk(true);
        }

    }

    float speed;


    void CallWalk(bool waitFoward)
    {
        if (waitFoward)
        {
            StartCoroutine(ChangeToWalkLater());
        }
        else
        {
            BeginMove();
        }
    }

    IEnumerator ChangeToWalkLater()
    {
        AnimatorClipInfo[] clips = anim.GetCurrentAnimatorClipInfo(0);
        if (clips.Length > 0)
        {
            float time = clips[0].clip.length;
            yield return new WaitForSeconds(time);
        }
        BeginMove();
    }


    void BeginMove()
    {
        speed = Random.Range(0f, 1f);
        anim.SetTrigger(move);
        anim.SetFloat(moveSpeed, speed);
    }


    void CallWakeUp(bool waitFoward)
    {
        if (waitFoward)
        {
            StartCoroutine(ChangeToWakeUpLater());
        }
        else
        {
            BeginWakeUp();
        }
    }

    IEnumerator ChangeToWakeUpLater()
    {
        AnimatorClipInfo[] clips = anim.GetCurrentAnimatorClipInfo(0);
        if (clips.Length > 0)
        {
            float time;
            AnimationClip clip = clips[0].clip;
            if (clip.isLooping)
            {
                time = clip.length * Random.Range(0.7f, 2f);
            }
            else
            {
                time = Time.deltaTime;
            }
            yield return new WaitForSeconds(time);
        }

        
        BeginWakeUp();
    }


    void BeginWakeUp()
    {
        anim.SetTrigger(wakeUp);
        //CallWalk(true);
    }






}
