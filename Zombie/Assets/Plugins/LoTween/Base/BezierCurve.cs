using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class BezierCurve
{
    [SerializeField]Vector3 _start;
    [SerializeField]Vector3 _end;

    [SerializeField]Vector3 _startTangent;
    [SerializeField]Vector3 _endTangent;

    [SerializeField]BezierCurve _head;
    [SerializeField]BezierCurve _tail;

    [SerializeField]NodeMode _startMode = NodeMode.Aligned;
    [SerializeField]NodeMode _endMode = NodeMode.Aligned;

    [SerializeField]Transform _basedObject;

    /// <summary>
    /// 曲线的长度.
    /// </summary>
    public float length;

    [SerializeField]float startSlope;
    [SerializeField]float endSlope;


    /// <summary>
    /// 依赖的Transform.
    /// </summary>
    public Transform basedObject {
        get {
            if (_head)
                return _head.basedObject;
            else
                return _basedObject;
        }
        set {
            _basedObject = value;
        }
    }

    /// <summary>
    /// 起点.
    /// </summary>
    public Vector3 start
    {
        get { return _start; }
        set
        {
            _start = value;
            if (_head) { _head._end = _start; }

            length = GetLineLength();
        }
    }

    /// <summary>
    /// 终点.
    /// </summary>
    public Vector3 end
    {
        get { return _end; }
        set
        {
            _end = value;
            if (_tail) _tail._start = _end;

            length = GetLineLength();
        }
    }

    /// <summary>
    /// 起点切线控制点.
    /// </summary>
    public Vector3 startTangent
    {
        get { return _startTangent + _start; }
        set
        {
            _startTangent = value - _start;
            if (_head)
            {
                if (_startMode == NodeMode.Aligned)
                {
                    _head._endTangent = -_startTangent;
                }
            }

            length = GetLineLength();
        }
    }

    /// <summary>
    /// 终点切线控制点.
    /// </summary>
    public Vector3 endTangent
    {
        get { return _endTangent + _end; }
        set
        {
            _endTangent = value - _end;
            if (_tail)
            {
                if (_endMode == NodeMode.Aligned)
                {
                    _tail._startTangent = -_endTangent;
                }
            }

            length = GetLineLength();
        }
    }

    /// <summary>
    /// 链接的上一个节点.
    /// </summary>
    public BezierCurve headNode
    {
        get { return _head; }
        set
        {
            _head = value;
            if (value)
            {
                _startMode = _head.endNodeMode;
                _head._tail = this;
                _start = _head._end;
                if (_startMode == NodeMode.Aligned) _startTangent = -_head._endTangent;
            }
            length = GetLineLength();
        }
    }

    /// <summary>
    /// 链接的下一个节点.
    /// </summary>
    public BezierCurve tailNode
    {
        get { return _tail; }
        set {
            _tail = value;
            if (value)
            {
                _endMode = _tail.startNodeMode;
                _tail._head = this;
                _end = _tail._start;
                if (_endMode == NodeMode.Aligned) _endTangent = -_tail._startTangent;
            }
            

            length = GetLineLength();
        }
    }

    /// <summary>
    /// 起点节点的控制模式.
    /// </summary>
    public NodeMode startNodeMode {
        get { return _startMode; }
        set {
            _startMode = value;
            if (_head) _head._endMode = value;
        }
    }

    /// <summary>
    /// 终点节点的控制模式.
    /// </summary>
    public NodeMode endNodeMode {
        get { return _endMode; }
        set {
            _endMode = value;
            if (_tail) _tail._startMode = value;
        }
    }


    float GetLineLength()
    {
        float len = 0;
        int count = 100;
        for (int i = 0; i < count - 1; i++)
        {
            len += Vector3.Distance(GetBezierPos(start, startTangent, endTangent, end, (float)i / count), GetBezierPos(start, startTangent, endTangent, end, (float)(i + 1) / count));
        }

        float eachLen = len / count;

        startSlope = Vector3.Distance(start, GetBezierPos(start, startTangent, endTangent, end, 1.0f / count)) / eachLen;
        endSlope = Vector3.Distance(end, GetBezierPos(start, startTangent, endTangent, end, (float)(count - 1) / count)) / eachLen;
        return len;
    }

    public float GetLinerTime(float time)
    {
        return time / Mathf.Lerp(startSlope, endSlope, time);
    }



    public BezierCurve(Vector3 start, Vector3 end)
    {
        this._start = start;
        this._end = end;
        this.startTangent = GetBezierPos(start, end, 1f / 3f);
        this.endTangent = GetBezierPos(start, end, 2f / 3f);
    }

    public BezierCurve(Vector3 start, Vector3 end, Transform based)
    {
        this._start = start;
        this._end = end;
        this.startTangent = GetBezierPos(start, end, 1f / 3f);
        this.endTangent = GetBezierPos(start, end, 2f / 3f);
        basedObject = based;
    }

    public static BezierCurve GetTail(BezierCurve source)
    {
        BezierCurve result = new BezierCurve(source.end, source.end * 2 - source.start);
        //result.startNodeMode = source.endNodeMode;
        result.headNode = source;
        return result;
    }

    public static BezierCurve InsertCurve(BezierCurve resource)
    {
        BezierCurve result = new BezierCurve(resource.start, GetBezierPos(resource.start, resource.startTangent, resource.endTangent, resource.end, 0.5f));

        result.headNode = resource.headNode;
        resource.headNode = result;

        return result;
    }

    public static void RemoveCurve(BezierCurve target, bool isStart)
    {
        BezierCurve head = target.headNode;
        BezierCurve tail = target.tailNode;
        head.tailNode = tail;
        if (isStart)
            head.end = target.end;
    }

    [System.Serializable]
    public enum NodeMode
    {
        Free, Aligned,
    }


#if UNITY_EDITOR 

    //当前选择的点.
    public static Vector3 selectPoint = Vector3.negativeInfinity;

    // 是否绘制位置文本框.
    public static bool drawPointPosLable = false;

    // 锁定Z轴.
    public static bool lockAxisZ = false;

    //Editor下进行画线.
    public bool DrawLine(Quaternion handleRotation, Color lineColor, Color handleLineColor, Color handleColor)
    {
        Vector3 startPoint = start;
        Vector3 endPoint = end;
        Vector3 startTangentPoint = startTangent;
        Vector3 endTangentPoint = endTangent;
        
        if (basedObject)
        {
            if (headNode == null)
            {
                startPoint = basedObject.position;
            }
            else
            {
                startPoint = basedObject.TransformPoint(start);
            }
            endPoint = basedObject.TransformPoint(end);
            startTangentPoint = basedObject.TransformPoint(startTangent);
            endTangentPoint = basedObject.TransformPoint(endTangent);
        }




        EditorGUI.BeginChangeCheck();

        if (headNode == null && !basedObject)
        {
            startPoint = Handles.DoPositionHandle(startPoint, handleRotation);
        }

        Handles.color = handleColor;
        // 当选中Handle时才绘制坐标系.
        if (Handles.Button(startTangentPoint, handleRotation, HandleUtility.GetHandleSize(startTangentPoint) * 0.04f, HandleUtility.GetHandleSize(startTangentPoint) * 0.06f, Handles.DotHandleCap))
        {
            selectPoint = startPoint;
        }

        if (Handles.Button(endTangentPoint, handleRotation, HandleUtility.GetHandleSize(endTangentPoint) * 0.04f, HandleUtility.GetHandleSize(endTangentPoint) * 0.06f, Handles.DotHandleCap))
        {
            selectPoint = endPoint;
        }

        // 显示坐标偏差.
        Vector3 offsetPos = basedObject ? (basedObject.position - basedObject.localPosition) : Vector3.zero;

        // 当选中点的位置是起点/终点时才显示坐标系.
        if (selectPoint == startPoint)
        {
            startTangentPoint = Handles.DoPositionHandle(startTangentPoint, handleRotation);
            Handles.Label(startTangentPoint, (startTangentPoint - offsetPos).ToString("0.00"));
        }

        if (selectPoint == endPoint)
        {
            endTangentPoint = Handles.DoPositionHandle(endTangentPoint, handleRotation);
            Handles.Label(endTangentPoint, (endTangentPoint - offsetPos).ToString("0.00"));
        }

        if (drawPointPosLable)
        {
            if (_head == null)
                Handles.Label(startPoint, (startPoint - offsetPos).ToString("0.00"));
            Handles.Label(endPoint, (endPoint - offsetPos).ToString("0.00"));
        }

        Handles.color = handleLineColor;
        Handles.DrawLine(startPoint, startTangentPoint);
        Handles.DrawLine(endPoint, endTangentPoint);

        endPoint = Handles.DoPositionHandle(endPoint, handleRotation);
        
        
        Handles.DrawBezier(startPoint, endPoint, startTangentPoint, endTangentPoint, lineColor, null, 2.5f);
        if (EditorGUI.EndChangeCheck())
        {
            if (basedObject)
            {
                if (lockAxisZ)
                {
                    startTangentPoint.z = basedObject.position.z;
                    endTangentPoint.z = basedObject.position.z;
                    endPoint.z = basedObject.position.z;
                }
                startTangent = basedObject.InverseTransformPoint(startTangentPoint);
                endTangent = basedObject.InverseTransformPoint(endTangentPoint);
                end = basedObject.InverseTransformPoint(endPoint);
            }
            else
            {
                if (lockAxisZ)
                {
                    startTangentPoint.z = start.z;
                    endTangentPoint.z = start.z;
                    endPoint.z = start.z;
                }
                startTangent = startTangentPoint;
                endTangent = endTangentPoint;
                end = endPoint;
            }

            if (headNode == null && !basedObject)
            {
                if (lockAxisZ)
                {
                    startPoint.z = start.z;
                }
                start = startPoint;
            }
            return true;
        }

        return false;

    }
#endif




    public static Vector3 GetBezierPos(Vector3 p0, Vector3 p1, float t)
    {
        return (1 - t) * p0 + t * p1;
    }

    public static Vector3 GetBezierPos(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        return Mathf.Pow(1 - t, 2) * p0 + 2 * (1 - t) * t * p1 + Mathf.Pow(t, 2) * p2;
    }

    public static Vector3 GetBezierPos(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        return Mathf.Pow(1 - t, 3) * p0 + 3 * Mathf.Pow(1 - t, 2) * t * p1 + 3 * Mathf.Pow(t, 2) * (1 - t) * p2 + Mathf.Pow(t, 3) * p3;
    }
    
    public static implicit operator bool(BezierCurve exist)
    {
        return exist != null;
    }


}
