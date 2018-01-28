using UnityEngine;
using System.Collections;
using UnityEditor;

namespace LoGameFrame
{

    [CustomEditor(typeof(LoTweenColor), true)]
    public class LoTweenColorEditor : LoTweenSingleBaseEditor
    {

        LoTweenColor tc;

        protected override void OnEnable()
        {
            base.OnEnable();

            tc = (LoTweenColor)target;

            Undo.IncrementCurrentGroup();
            Undo.SetCurrentGroupName("LoTweenColor");
        }

        public override void OnInspectorGUI()
        {

            tc.From = EditorGUILayout.ColorField("From", tc.From);
            tc.To = EditorGUILayout.ColorField("To", tc.To);

            base.OnInspectorGUI();
        }


    }

}