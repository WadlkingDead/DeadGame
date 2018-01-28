using UnityEngine;
using System.Collections;
using LoGameFrame;

public class LoTweenPositionMulty : LoTweenMultyBase {

    [SerializeField, HideInInspector]
    Vector3 from;

    [SerializeField, HideInInspector]
    Vector3 delta;

    [SerializeField, HideInInspector]
    Vector3 to;


    public Vector3 From
    {
        get
        {
            return from;
        }
        set
        {
            from = value;
            hidenDelta = To - from;
        }
    }


    public Vector3 Delta
    {
        get
        {
            return delta;
        }
        set
        {
            delta = value;
        }
    }


    public Vector3 To
    {
        get {
            return to;
        }
        set {
            to = value;
            hidenDelta = to - From;
        }
    }


    [SerializeField, HideInInspector]
    Vector3 hidenDelta;


    [SerializeField, HideInInspector]
    public AnimationCurve selfUseX = new AnimationCurve();

    [SerializeField, HideInInspector]
    public AnimationCurve selfUseY = new AnimationCurve();

    [SerializeField, HideInInspector]
    public AnimationCurve selfUseZ = new AnimationCurve();


    [SerializeField]
    AnimationCurveArray3 _curves;

    public AnimationCurveArray3 Curves {
        get {
            CheckCurvesIsNull();
            return _curves;
        }
        set {
            _curves = value;

            if (value == null)
            {
                return;
            }

            curves = new AnimationCurve[3];
            curves[0] = _curves.x;
            curves[1] = _curves.y;
            curves[2] = _curves.z;
        }
    }

    void CheckCurvesIsNull()
    {
        if (_curves == null)
        {
            _curves = new AnimationCurveArray3();
        }
    }

    public void SetCurveX(AnimationCurve curve)
    {
        CheckCurvesIsNull();
        _curves.x = curve;
        curves[0] = curve;
    }

    public void SetCurveY(AnimationCurve curve)
    {
        CheckCurvesIsNull();
        _curves.y = curve;
        curves[1] = curve;
    }

    public void SetCurveZ(AnimationCurve curve)
    {
        CheckCurvesIsNull();
        _curves.z = curve;
        curves[2] = curve;
    }

    public void SetCurveX(string curveName)
    {
        CheckCurvesIsNull();
        _curves.x = GetCurve(curveName);
        curves[0] = _curves.x;
    }

    public void SetCurveY(string curveName)
    {
        CheckCurvesIsNull();
        _curves.y = GetCurve(curveName);
        curves[1] = _curves.y;
    }

    public void SetCurveZ(string curveName)
    {
        CheckCurvesIsNull();
        _curves.z = GetCurve(curveName);
        curves[2] = _curves.z;
    }


    bool isUI = false;


    protected override void OnEnable()
    {
        base.OnEnable();
        isUI = GetComponent<RectTransform>() != null;

        if (deltaValues == null)
        {
            deltaValues = new float[3];
        }

        if (selfUseX != null && selfUseY != null && selfUseZ != null)
        {
            Curves = new AnimationCurveArray3(selfUseX, selfUseY, selfUseZ);
        }
    }



    RectTransform mRTran;
    RectTransform rectTransform
    {
        get
        {
            if (mRTran == null)
            {
                mRTran = GetComponent<RectTransform>();
            }
            return mRTran;
        }
    }

    public bool changeGlobal = false;

    Vector3 tempPosition;
    protected override void UpdateValue(float[] delta, float timer)
    {
        tempPosition.x = LoTweenControl.Lerp(From.x, (From + Delta).x, delta[0]) + hidenDelta.x * timer;
        tempPosition.y = LoTweenControl.Lerp(From.y, (From + Delta).y, delta[1]) + hidenDelta.y * timer;
        tempPosition.z = LoTweenControl.Lerp(From.z, (From + Delta).z, delta[2]) + hidenDelta.z * timer;

        if (changeGlobal)
        {
            transform.position = tempPosition;
        }
        else
        {
            if (isUI)
            {
                rectTransform.anchoredPosition3D = tempPosition;
            }
            else
            {
                transform.localPosition = tempPosition;
            }
        }
    }


    public override void Clone(LoTweenBase target)
    {
        base.Clone(target);
        LoTweenPositionMulty tt = (LoTweenPositionMulty)target;
        this.From = tt.From;
        this.Delta = tt.Delta;
        this.To = tt.To;
        this.changeGlobal = tt.changeGlobal;
        this.Curves = tt.Curves;
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

}
