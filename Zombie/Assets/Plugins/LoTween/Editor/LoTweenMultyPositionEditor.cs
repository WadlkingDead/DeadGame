using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

namespace LoGameFrame
{

    [CustomEditor(typeof(LoTweenPositionMulty))]
    public class LoTweenMultyPositionEditor : Editor
    {

        public LoTweenPositionMulty tp;

        void OnEnable()
        {
            tp = (LoTweenPositionMulty)target;

            Undo.IncrementCurrentGroup();
            Undo.SetCurrentGroupName("LoTweenMultyPosition");
        }

        bool isShowCurves = false;

        public override void OnInspectorGUI()
        {
            if (Application.isPlaying)
            {
                if (GUILayout.Button("Play"))
                {
                    tp.PlayFoward();
                }
            }


            base.OnInspectorGUI();

            Undo.RecordObject(tp, "LoTweenMultyPosition");

            DrawCurve();
        }

        [SerializeField]
        GameObject from;
        GameObject From
        {
            get
            {
                return from;
            }
            set
            {
                from = value;
                if (value == null)
                {
                    return;
                }
                if (tp.changeGlobal)
                {
                    tp.From = value.transform.position;
                }
                else
                {
                    tp.From = value.transform.localPosition;
                }
            }
        }

        [SerializeField]
        GameObject to;
        GameObject To
        {
            get
            {
                return to;
            }
            set
            {
                to = value;
                if (value == null)
                {
                    return;
                }
                if (tp.changeGlobal)
                {
                    tp.To = value.transform.position;
                }
                else
                {
                    tp.To = value.transform.localPosition;
                }
            }
        }


        float buttonWidth = 100;

        void DrawCurve()
        {
            tp.duration = EditorGUILayout.FloatField("Duration", tp.duration);

            GUILayout.BeginHorizontal();
            tp.From = EditorGUILayout.Vector3Field("From", tp.From);

            From = EditorGUILayout.ObjectField(From, typeof(GameObject), true, GUILayout.MaxWidth(50)) as GameObject;

            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();

            tp.To = EditorGUILayout.Vector3Field("To", tp.To);

            To = EditorGUILayout.ObjectField(To, typeof(GameObject), true, GUILayout.MaxWidth(50)) as GameObject;

            GUILayout.EndHorizontal();

            tp.Delta = EditorGUILayout.Vector3Field("Delta", tp.Delta);


            tp.selfUseX = EditorGUILayout.CurveField("Animation Curve.x", tp.selfUseX, GUILayout.MinWidth(30f), GUILayout.MinHeight(30f));
            tp.selfUseY = EditorGUILayout.CurveField("Animation Curve.y", tp.selfUseY, GUILayout.MinWidth(30f), GUILayout.MinHeight(30f));
            tp.selfUseZ = EditorGUILayout.CurveField("Animation Curve.z", tp.selfUseZ, GUILayout.MinWidth(30f), GUILayout.MinHeight(30f));

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("保存 Curve.x", GUILayout.MinWidth(buttonWidth)))
            {
                SaveCurveWindow.Init(tp.selfUseX);
            }

            if (GUILayout.Button("保存 Curve.y", GUILayout.MinWidth(buttonWidth)))
            {
                SaveCurveWindow.Init(tp.selfUseY);
            }

            if (GUILayout.Button("保存 Curve.z", GUILayout.MinWidth(buttonWidth)))
            {
                SaveCurveWindow.Init(tp.selfUseZ);
            }

            GUILayout.EndHorizontal();

            GUILayout.Space(5);


            isShowCurves = LoTweenTools.DrawHeader("ShowAllCurves", "ShowAllCurves", false, false);


            if (isShowCurves)
            {

                tweenObject = AssetDatabase.LoadAssetAtPath<LoTweenObject>(LoTweenOption.curveFilePath);

                tweenObject.UpdateCurve();

                foreach (var cc in LoCurveStor.allCurves)
                {
                    GUILayout.BeginHorizontal("box");
                    AnimationCurve tempCurve = LoTweenOption.CopyCurve(cc.Value);
                    EditorGUILayout.CurveField(cc.Key, tempCurve, GUILayout.MinHeight(50f), GUILayout.MinWidth(50f));
                    GUILayout.BeginVertical();
                    if (GUILayout.Button("Apply X"))
                    {
                        tp.selfUseX = cc.Value;
                    }
                    if (GUILayout.Button("Apply Y"))
                    {
                        tp.selfUseY = cc.Value;
                    }
                    if (GUILayout.Button("Apply Z"))
                    {
                        tp.selfUseZ = cc.Value;
                    }
                    GUILayout.EndVertical();
                    GUILayout.EndHorizontal();

                    GUILayout.Space(10f);
                }

                if (GUILayout.Button("Close"))
                {
                    isShowCurves = false;
                    EditorPrefs.SetBool("ShowAllCurves", false);
                }
            }


        }


        LoTweenObject tweenObject;



    }


    public class SaveCurveWindow : EditorWindow
    {
        static string saveName = "SaveCurveName";

        public static AnimationCurve savedCurve;
        static SaveCurveWindow window;
        public static void Init(AnimationCurve curve)
        {
            savedCurve = curve;
            window = GetWindow<SaveCurveWindow>("请输入曲线名");
            window.minSize = new Vector2(200f, 60f);
            window.maxSize = new Vector2(201f, 61f);
        }

        void OnEnable()
        {
            string tempName = PlayerPrefs.GetString(saveName);
            if (!string.IsNullOrEmpty(tempName))
            {
                curveName = tempName;
            }
        }


        [SerializeField]
        string curveName = "请输入曲线名";

        void OnGUI()
        {
            curveName = GUILayout.TextField(curveName);

            GUILayout.Space(10f);

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("确定", GUILayout.Width(50)))
            {
                if (LoTweenObjectEditor.TryAddCurve(curveName, savedCurve))
                {
                    PlayerPrefs.SetString(saveName, curveName);
                    window.Close();
                }
            }

            GUILayout.Space(10f);

            if (GUILayout.Button("取消", GUILayout.Width(50)))
            {
                window.Close();
            }
        }

    }
}