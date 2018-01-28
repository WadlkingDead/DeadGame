using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace LoGameFrame
{
    [CustomEditor(typeof(LoTweenPath))]
    public class LoTweenPathEditor : LoTweenSingleBaseEditor
    {

        LoTweenPath tp;

        // 线的颜色.
        Color lineColor = Color.white;
        // 控制点颜色.
        Color handleColor = Color.red;
        // 控制线的颜色.
        Color handleLineColor = Color.green;

        // 旋转角度.
        Quaternion handleRotation;

        // 字体颜色.
        GUIStyle fontStyle;

        protected override void OnEnable()
        {
            base.OnEnable();
            tp = (LoTweenPath)target;

            if (tp.Count == 0)
            {
                float sizeScale = 1;
                Canvas canvas = tp.transform.GetComponentInParent<Canvas>();
                if (canvas)
                {
                    sizeScale = 100;
                }
                BezierCurve curve = new BezierCurve(Vector3.zero, Vector3.right * sizeScale, tp.transform);
                tp.AddTail(curve);
            }

            if (tp.Count > 1)
            {
                for (int i = 0; i < tp.Count - 1; i++)
                {
                    tp[i].tailNode = tp[i + 1];
                }
            }

            BezierCurve.selectPoint = Vector3.negativeInfinity;
            fontStyle = new GUIStyle();
            fontStyle.fontStyle = FontStyle.Bold;

            tp.editorPlayStart = OnStartPlay;
        }

        
        void OnDestroy()
        {
            // 不需要显示时销毁临时Transform.
            if (tempBasedTrans) DestroyImmediate(tempBasedTrans.gameObject);
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            tp.lookToTarget = EditorGUILayout.Toggle("LookToTarget", tp.lookToTarget);
            if (tp.lookToTarget)
            {
                tp.reverseLook = EditorGUILayout.Toggle("ReverseLook", tp.reverseLook);
                tp.followAxis = (LoTweenPath.Axis)EditorGUILayout.EnumPopup("FollowAxis", tp.followAxis);
            }

            #region CurveParams

            if (!Application.isPlaying)
            {
                GUILayout.Space(5);
                GUILayout.Label("Curve Params:");
                EditorGUILayout.BeginVertical("box");
                {
                    lineColor = EditorGUILayout.ColorField("LineColor", lineColor);
                    handleColor = EditorGUILayout.ColorField("HandleColor", handleColor);
                    handleLineColor = EditorGUILayout.ColorField("HandleLineColor", handleLineColor);
                    fontStyle.normal.textColor = EditorGUILayout.ColorField("TextColor", fontStyle.normal.textColor);
                    BezierCurve.lockAxisZ = EditorGUILayout.Toggle("Lock Axis Z", BezierCurve.lockAxisZ);

                    EditorGUILayout.EndVertical();
                }

                GUILayout.Space(5);
                GUILayout.Label("Curve Detail:");
                EditorGUILayout.BeginVertical("box");
                {
                    // 起点.
                    EditorGUILayout.BeginHorizontal("box");
                    EditorGUILayout.LabelField("Start Point", GUILayout.Width(70));
                    EditorGUILayout.Vector3Field("", tp.transform.localPosition);
                    EditorGUILayout.EndVertical();

                    Vector3 offsetPos = tp.transform.position - tp.transform.localPosition;

                    // 中间节点.
                    for (int i = 1; i < tp.Count; i++)
                    {
                        EditorGUILayout.BeginHorizontal("box");
                        EditorGUILayout.LabelField("Point " + i.ToString("00") + ": ", GUILayout.Width(65));
                        Vector3 startPoint = tp.transform.InverseTransformPoint(EditorGUILayout.Vector3Field("", tp.transform.TransformPoint(tp[i].start) - offsetPos, GUILayout.Width(180))) + offsetPos;
                        if (GUI.changed) tp[i].start = startPoint;
                        tp[i].startNodeMode = (BezierCurve.NodeMode)EditorGUILayout.EnumPopup(tp[i].startNodeMode, GUILayout.Width(58));
                        if (GUILayout.Button("Delete", GUILayout.Width(50)))
                        {
                            tp.RemoveCurve(i);
                        }
                        if (GUILayout.Button("Insert Foward"))
                        {
                            tp.InsertCurve(i);
                        }
                        EditorGUILayout.EndHorizontal();
                    }

                    // 终点.
                    EditorGUILayout.BeginHorizontal("box");
                    EditorGUILayout.LabelField("End Point", GUILayout.Width(65));
                    Vector3 endPoint = tp.transform.InverseTransformPoint(EditorGUILayout.Vector3Field("", tp.transform.TransformPoint(tp.LastCurve.end) - offsetPos, GUILayout.Width(180))) + offsetPos;
                    if (GUI.changed) tp.LastCurve.end = endPoint;

                    if (GUILayout.Button("Delete", GUILayout.Width(50)))
                    {
                        tp.RemoveCurve(tp.Count);
                    }
                    if (GUILayout.Button("Insert Foward"))
                    {
                        tp.InsertCurve(tp.Count);
                    }

                    EditorGUILayout.EndHorizontal();
                    if (GUILayout.Button("Add Tail"))
                    {
                        tp.AddTail();
                    }
                    EditorGUILayout.EndVertical();
                }
            }

            #endregion
        }

        void OnSceneGUI()
        {
            handleRotation = Tools.pivotRotation == PivotRotation.Local ? tp.FirstCurve.basedObject.localRotation : Quaternion.identity;
            
            for (int i = 0; i < tp.Count; i++)
            {
                Undo.RecordObject(tp, "TweenPathNode");
                if (tp[i].DrawLine(handleRotation, lineColor, handleLineColor, handleColor))
                    EditorUtility.SetDirty(tp);
            }

            Handles.Label(tp.FirstCurve.basedObject.TransformPoint(tp.FirstCurve.start), "Start Point", fontStyle);
            for (int i = 1; i < tp.Count; i++)
            {
                Handles.Label(tp.FirstCurve.basedObject.TransformPoint(tp[i].start), "Point " + i.ToString("00"), fontStyle);
            }

            Handles.Label(tp.FirstCurve.basedObject.TransformPoint(tp.LastCurve.end), "End Point", fontStyle);
            Handles.color = Color.yellow;
            int count = 20;
            float each = 4.0f / count;
            Vector3 first = BezierCurve.GetBezierPos(p0, p1, p2, p3, 1.0f / count);
            float startSlope = first.x / 4.0f / (1.0f / count);

            Vector3 second = BezierCurve.GetBezierPos(p0, p1, p2, p3, 2.0f / count);
            float secondSlope = (second.x - first.x) / 4.0f / (1.0f / count);

            Vector3 last = BezierCurve.GetBezierPos(p0, p1, p2, p3, (float)(count - 1) / count);
            float endSlope = (1 - last.x / 4.0f) / (1.0f / count);
            endSlope = 1 / endSlope;

            Debug.Log(startSlope + " ==> " + secondSlope + ", " + endSlope);
            Vector3 start = Vector3.zero;
            for (int i = 1; i <= count; i++)
            {
                float time = (float)i / count;
                Vector3 result = BezierCurve.GetBezierPos(p0, p1, p2, p3, GetLinerTime(startSlope, endSlope, time));
                float y = result.x / 4;
                Vector3 end = new Vector3(time, y, 0) * 10;
                Handles.DrawLine(start, end);
                start = end;
            }

            Handles.color = Color.blue;
            Handles.DrawLine(Vector3.zero, new Vector3(1, 1, 0) * 10);
        }

        float GetLinerTime(float start, float end, float time)
        {
            return time;
            //return time / Mathf.Lerp(start, end, time);
            //return time * end;
        }


        Vector3 p0 = Vector3.zero;
        Vector3 p1 = new Vector3(0.2f, 0, 0);
        Vector3 p2 = new Vector3(3.8f, 0, 0);
        Vector3 p3 = new Vector3(4, 0, 0);

        Transform tempBasedTrans;
        // 开始播放时曲线基础Transform设置为临时的，防止路径跟着物体跑.
        void OnStartPlay()
        {
            if (!tempBasedTrans)
            {
                tempBasedTrans = new GameObject(tp.name + "(clone)").transform;
                tempBasedTrans.gameObject.hideFlags = HideFlags.HideAndDontSave;
                if (tp.transform.parent)
                    tempBasedTrans.SetParent(tp.transform.parent);

                tempBasedTrans.localPosition = tp.transform.localPosition;
                tempBasedTrans.localRotation = tp.transform.localRotation;
                tempBasedTrans.localScale = tp.transform.localScale;

                tp.FirstCurve.basedObject = tempBasedTrans;
            }
            
            
        }


    }
}

