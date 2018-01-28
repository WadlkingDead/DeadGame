using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LoGameFrame;


public class LoTweenRotation : LoTweenSingleBase
{

    [SerializeField]
    Quaternion from;

    [SerializeField]
    Quaternion to;

    public Quaternion From
    {
        get
        {
            return from;
        }
        set
        {
            from = value;
        }
    }

    public Quaternion To
    {
        get
        {
            return to;
        }
        set
        {
            to = value;
        }
    }

    protected override void UpdateValue(float percent)
    {
        transform.localRotation = LoTweenControl.Lerp(From, To, percent);
    }


    public override void ResetToBegin()
    {
        base.ResetToBegin();
        transform.localRotation = From;
    }

    public override void ResetToEnd()
    {
        base.ResetToEnd();
        transform.localRotation = To;
    }
}


