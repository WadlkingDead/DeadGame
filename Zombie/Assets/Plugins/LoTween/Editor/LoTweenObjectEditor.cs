using UnityEngine;
using System.Collections;
using UnityEditor;

namespace LoGameFrame
{

    [CustomEditor(typeof(LoTweenObject))]
    public class LoTweenObjectEditor : Editor
    {

        static LoTweenObject obj;

        void OnEnable()
        {
            obj = (LoTweenObject)target;
            Undo.IncrementCurrentGroup();
            Undo.SetCurrentGroupName("LoTweenObject");
        }


        public override void OnInspectorGUI()
        {
            Undo.RecordObject(obj, "LoTweenObject");

            GUILayout.Label("添加曲线:");

            obj.editorCurve = EditorGUILayout.CurveField(obj.editorCurve, GUILayout.MinWidth(50), GUILayout.MinHeight(50));
            obj.editorName = GUILayout.TextField(obj.editorName);

            string saveButtonName = "保存";
            if (obj.names.Contains(obj.editorName))
                saveButtonName = "覆盖";

            if (GUILayout.Button(saveButtonName))
            {
                TryAddCurve(obj.editorName, obj.editorCurve);

            }

            GUILayout.Space(10f);

            GUILayout.Label("所有曲线:");

            for (int i = 0; i < obj.names.Count; i++)
            {
                EditorGUILayout.BeginVertical("box");
                {
                    GUILayout.BeginHorizontal();

                    AnimationCurve tempCurve = LoTweenOption.CopyCurve(obj.curves[i]);
                    EditorGUILayout.CurveField(obj.names[i], tempCurve, GUILayout.MinWidth(30f), GUILayout.MinHeight(40f));

                    GUILayout.BeginVertical();

                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button("编辑", GUILayout.Width(60)))
                    {
                        obj.editorCurve = tempCurve;
                        obj.editorName = obj.names[i];
                    }

                    if (GUILayout.Button("上移") && i > 0)
                    {
                        string tempName = obj.names[i];
                        obj.names[i] = obj.names[i - 1];
                        obj.names[i - 1] = tempName;
                        obj.curves[i] = obj.curves[i - 1];
                        obj.curves[i - 1] = tempCurve;
                    }

                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button("删除", GUILayout.Width(60)))
                    {
                        if (EditorUtility.DisplayDialog("警告！", "确定删除曲线“" + obj.names[i] + "”?", "是", "否"))
                        {
                            obj.RemoveCurve(obj.names[i]);
                            RefreshAsset();
                            return;
                        }
                        else
                        {
                            return;
                        }
                    }

                    if (GUILayout.Button("下移") && i < obj.names.Count - 1)
                    {
                        string tempName = obj.names[i];
                        obj.names[i] = obj.names[i + 1];
                        obj.names[i + 1] = tempName;
                        obj.curves[i] = obj.curves[i + 1];
                        obj.curves[i + 1] = tempCurve;
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.EndVertical();

                    GUILayout.EndHorizontal();

                    GUILayout.Space(5);
                }
                EditorGUILayout.EndVertical();
            }

        }


        public static bool TryAddCurve(string name, AnimationCurve curve)
        {
            if (obj == null)
            {
                obj = AssetDatabase.LoadAssetAtPath<LoTweenObject>(LoTweenOption.curveFilePath);
            }


            if (name.Equals("请输入曲线名") || string.IsNullOrEmpty(name))
            {
                EditorUtility.DisplayDialog("错误提醒！", "请输入曲线名！", "关闭");
                return false;
            }
            for (int i = 0; i < obj.names.Count; i++)
            {
                if (name.Equals(obj.names[i]))
                {
                    if (EditorUtility.DisplayDialog("警告！", "已经存在该曲线名，是否覆盖？", "是", "否"))
                    {
                        obj.AddCurve(name, curve);
                        RefreshAsset();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }


            }
            obj.AddCurve(name, curve);
            RefreshAsset();
            return true;
        }

        static void RefreshAsset()
        {
            LoTweenObject newObj = ScriptableObject.CreateInstance<LoTweenObject>();
            newObj.names = obj.names;
            newObj.curves = obj.curves;

            AssetDatabase.CreateAsset(newObj, LoTweenOption.curveFilePath);
            AssetDatabase.Refresh();
            Selection.activeObject = newObj;
        }


    }
}