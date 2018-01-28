using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

namespace LoGameFrame
{

    [CustomEditor(typeof(LoTweenSingleBase), false)]
    public class LoTweenSingleBaseEditor : Editor
    {

        LoTweenSingleBase baseTween;

        protected virtual void OnEnable()
        {
            baseTween = (LoTweenSingleBase)target;

            Undo.IncrementCurrentGroup();
            Undo.SetCurrentGroupName("LoTweenSingleBase");
        }

        bool isShowAllCurves = false;
        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();

            Undo.RecordObject(baseTween, "LoTweenSingleBase");

            if (Application.isPlaying)
            {
                GUILayout.BeginVertical("box");
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("PlayFoward"))
                {
                    baseTween.PlayFoward();
                }

                if (GUILayout.Button("PlayReverse"))
                {
                    baseTween.PlayReverse();
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();

                if (baseTween.pause)
                {
                    if (GUILayout.Button("Resume"))
                    {
                        baseTween.pause = false;
                    }
                }
                else
                {
                    if (GUILayout.Button("Pause"))
                    {
                        baseTween.pause = true;
                    }
                }
                

                if (GUILayout.Button("Stop"))
                {
                    baseTween.Stop();
                }

                GUILayout.EndHorizontal();


                GUILayout.BeginHorizontal();
                if (GUILayout.Button("ResetToBegin"))
                {
                    baseTween.ResetToBegin();
                }

                if (GUILayout.Button("ResetToEnd"))
                {
                    baseTween.ResetToEnd();
                }

                GUILayout.EndHorizontal();

                GUILayout.EndVertical();
            }

            DrawBaseCurves();

        }


        public void DrawBaseCurves()
        {
            EditorGUILayout.BeginVertical("box");

            baseTween.playOnEnable = EditorGUILayout.Toggle("Play OnEnable", baseTween.playOnEnable);
            baseTween.useTimeScale = EditorGUILayout.Toggle("Use TimeScale", baseTween.useTimeScale);

            baseTween.playType = (LoTweenBase.PlayType)EditorGUILayout.EnumPopup("Play Type", baseTween.playType);
            if (baseTween.playType != LoTweenBase.PlayType.Clamp)
            {
                baseTween.repeatTimes = EditorGUILayout.IntField("RepeatTimes", baseTween.repeatTimes);
            }

            baseTween.duration = EditorGUILayout.FloatField("Duration", baseTween.duration);
            baseTween.Curve = EditorGUILayout.CurveField("Animation Curve", baseTween.Curve, GUILayout.Height(70));

            EditorGUILayout.EndVertical();

            if (GUILayout.Button("保存 Animation Curve"))
            {
                SaveCurveWindow.Init(baseTween.Curve);
            }

            GUILayout.Space(5);

            isShowAllCurves = LoTweenTools.DrawHeader("ShowAllCurves", "ShowAllCurves", false, false);
            if (isShowAllCurves)
            {

                tweenObject = AssetDatabase.LoadAssetAtPath<LoTweenObject>(LoTweenOption.curveFilePath);

                tweenObject.UpdateCurve();

                foreach (var cc in LoCurveStor.allCurves)
                {
                    GUILayout.BeginHorizontal("box");
                    AnimationCurve tempCurve = LoTweenOption.CopyCurve(cc.Value);
                    EditorGUILayout.CurveField(cc.Key, tempCurve, GUILayout.Height(60f));
                    if (GUILayout.Button("Select"))
                    {
                        baseTween.Curve = cc.Value;
                    }
                    GUILayout.EndHorizontal();
                }

                if (GUILayout.Button("关闭列表"))
                {
                    isShowAllCurves = false;
                    EditorPrefs.SetBool("ShowAllCurves", false);
                }
            }

        }


        LoTweenObject tweenObject;




    }
}