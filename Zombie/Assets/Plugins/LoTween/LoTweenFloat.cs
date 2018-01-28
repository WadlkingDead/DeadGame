using UnityEngine;
using System.Collections;
using LoGameFrame;

public class LoTweenFloat : LoTweenSingleBase
{

    [SerializeField]
    float from;

    [SerializeField]
    float to;

    public float From
    {
        get { return from; }
        set { from = value; }
    }

    public float To
    {
        get { return to; }
        set { to = value; }
    }

    public System.Action<float> onValueChange;

    protected override void UpdateValue(float percent)
    {
        if (onValueChange != null)
        {
            if (fixedValue)
                onValueChange.Invoke(percent);
            else
                onValueChange.Invoke(LoTweenControl.Lerp(From, To, percent));
        }
    }

}
