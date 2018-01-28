using UnityEngine;
using System.Collections;
using LoGameFrame;

public class LoTweenScale : LoTweenSingleBase {

    [SerializeField]
    Vector3 from;

    [SerializeField]
    Vector3 to;
	
	public Vector3 From {
		get { return from; }
		set { from = value; }
	}
	
	public Vector3 To {
		get { return to; }
		set { to = value; }
	}

	protected override void UpdateValue (float percent)
	{
        if (fixedValue)
            transform.localScale = Vector3.one * percent;
        else
            transform.localScale = LoTweenControl.Lerp(From, To, percent);
	}


    public override void ResetToBegin()
    {
        base.ResetToBegin();
        transform.localScale = From;
    }

    public override void ResetToEnd()
    {
        base.ResetToEnd();
        transform.localScale = To;
    }

    public static void ScaleTo(Transform tran, Vector3 from, Vector3 to, float duration, string curve, System.Action onFinished)
    {
        LoTweenScale ts = tran.gameObject.AddComponent<LoTweenScale>();
        ts.From = from;
        ts.To = to;
        ts.duration = duration;
        ts.Curve = ts.GetCurve(curve);
        ts.onFinished = (()=> {
            GameObject.DestroyImmediate(ts);
        });
        ts.onFinished += onFinished;
        ts.PlayFoward();
    }

    public static void ScaleTo(Transform tran, Vector3 to, float duration)
    {
        ScaleTo(tran, tran.localScale, to, duration, "line", null);
    }

    public static void ScaleTo(Transform tran, Vector3 from, Vector3 to, float duration)
    {
        ScaleTo(tran, from, to, duration, "line", null);
    }

    public static void ScaleTo(Transform tran, Vector3 from, Vector3 to, float duration, string curve)
    {
        ScaleTo(tran, from, to, duration, curve, null);
    }


}
