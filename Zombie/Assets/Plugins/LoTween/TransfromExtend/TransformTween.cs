using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformTween {

    public static void ScaleTo(this Transform tran, Vector3 from, Vector3 to, float duration, string curve, System.Action onFinished)
    {
        LoTweenScale.ScaleTo(tran, from, to, duration, curve, onFinished);
    }

    public static void ScaleTo(this Transform tran, Vector3 to, float duration)
    {
        ScaleTo(tran, tran.localScale, to, duration, "line", null);
    }

    public static void ScaleTo(this Transform tran, Vector3 from, Vector3 to, float duration)
    {
        ScaleTo(tran, from, to, duration, "line", null);
    }

    public static void ScaleTo(this Transform tran, Vector3 from, Vector3 to, float duration, string curve)
    {
        ScaleTo(tran, from, to, duration, curve, null);
    }

    public static void MoveTo(this Transform tran, Vector3 from, Vector3 to, float duration, string curve, bool changeGlobo, System.Action onFinished)
    {
        LoTweenPosition.MoveTo(tran, from, to, duration, curve, changeGlobo, onFinished);
    }

    public static void MoveTo(this Transform tran, Vector3 to, float duration)
    {
        MoveTo(tran, tran.localPosition, to, duration, "line", false, null);
    }

    public static void MoveTo(this Transform tran, Vector3 from, Vector3 to, float duration)
    {
        MoveTo(tran, from, to, duration, "line", false, null);
    }

    public static void MoveTo(this Transform tran, Vector3 from, Vector3 to, float duration, string curve)
    {
        MoveTo(tran, from, to, duration, curve, false, null);
    }

    public static void MoveTo(this Transform tran, Vector3 from, Vector3 to, float duration, string curve, bool changeGlobo)
    {
        MoveTo(tran, from, to, duration, curve, false, null);
    }


}
