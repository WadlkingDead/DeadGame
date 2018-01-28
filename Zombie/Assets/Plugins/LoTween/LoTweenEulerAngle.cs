using UnityEngine;
using System.Collections;
using LoGameFrame;

public class LoTweenEulerAngle : LoTweenSingleBase {

    [SerializeField]
    Vector3 from;

    [SerializeField]
    Vector3 to;
	
	public Vector3 From {
		get {
			return from;
		}
		set {
			from = value;
		}
	}
	
	public Vector3 To {
		get {
			return to;
		}
		set {
			to = value;
		}
	}
	
	protected override void UpdateValue (float percent)
	{
		transform.localEulerAngles = LoTweenControl.Lerp(From, To, percent);
	}


    public override void ResetToBegin()
    {
        base.ResetToBegin();
        transform.localEulerAngles = From;
    }

    public override void ResetToEnd()
    {
        base.ResetToEnd();
        transform.localEulerAngles = To;
    }

}
