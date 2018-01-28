using UnityEngine;
using System.Collections;
using LoGameFrame;

public class LoTweenPosition: LoTweenSingleBase {

    [SerializeField, HideInInspector]
    Vector3 from;

    [SerializeField, HideInInspector]
    Vector3 to;

    [SerializeField, HideInInspector]
    Vector3 delta;

    public Vector3 From {
		get { return from; }
		set { from = value; }
	}

    
    public Vector3 To {
		get { return to; }
		set { to = value; }
	}

    public Vector3 Delta {
        get { return delta; }
        set { delta = value; }
    }

    bool isUI = false;


    protected override void OnEnable()
    {
        base.OnEnable();
        isUI = GetComponent<RectTransform>() != null;
    }

	RectTransform mRTran;
	RectTransform rectTransform{
		get{
			if (mRTran == null) {
				mRTran = GetComponent<RectTransform>();
			}
			return mRTran;
		}
	}

	public bool changeGlobal = false;

	protected override void UpdateValue (float percent)
	{
		if (changeGlobal) {
            if (fixedValue)
                transform.position = From + Delta * percent;
            else
                transform.position = LoTweenControl.Lerp(From, To, percent);
		}
		else{
			if (isUI) {
                if (fixedValue)
                    rectTransform.anchoredPosition3D = From + Delta * percent;
                else
                    rectTransform.anchoredPosition3D = LoTweenControl.Lerp(From, To, percent);
			}
			else{
                if (fixedValue)
                    transform.localPosition = From + Delta * percent;
                else
				    transform.localPosition = LoTweenControl.Lerp(From, To, percent);
			}
		}

	}

    public override void ResetToBegin()
    {
        base.ResetToBegin();
        if (changeGlobal)
            transform.position = From;
        else
            transform.localPosition = From;
    }

    public override void ResetToEnd()
    {
        base.ResetToEnd();
        if (changeGlobal)
            transform.position = To;
        else
            transform.localPosition = To;
    }


    public static void MoveTo(Transform tran, Vector3 from, Vector3 to, float duration, string curve, bool changeGlobo, System.Action onFinished)
    {
        LoTweenPosition tp = tran.gameObject.AddComponent<LoTweenPosition>();
        tp.From = from;
        tp.To = to;
        tp.duration = duration;
        tp.Curve = tp.GetCurve(curve);
        tp.onFinished = (() => {
            GameObject.DestroyImmediate(tp);
        });
        tp.changeGlobal = changeGlobo;
        tp.onFinished += onFinished;
        tp.PlayFoward();
    }

    public static void MoveTo(Transform tran, Vector3 to, float duration)
    {
        MoveTo(tran, tran.localPosition, to, duration, "line", false, null);
    }

    public static void MoveTo(Transform tran, Vector3 from, Vector3 to, float duration)
    {
        MoveTo(tran, from, to, duration, "line", false, null);
    }

    public static void MoveTo(Transform tran, Vector3 from, Vector3 to, float duration, string curve)
    {
        MoveTo(tran, from, to, duration, curve, false, null);
    }

    public static void MoveTo(Transform tran, Vector3 from, Vector3 to, float duration, string curve, bool changeGlobo)
    {
        MoveTo(tran, from, to, duration, curve, false, null);
    }


}
