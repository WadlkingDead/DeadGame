using UnityEngine;
using System.Collections;
using UnityEditor;

namespace LoGameFrame
{

    [CustomEditor(typeof(LoTweenAlpha), true)]
    public class LoTweenAlphaEditor : LoTweenSingleBaseEditor
    {

        LoTweenAlpha ts;

        protected override void OnEnable()
        {
            base.OnEnable();

            ts = (LoTweenAlpha)target;

            Undo.IncrementCurrentGroup();
            Undo.SetCurrentGroupName("LoTweenAlpha");
        }

        public override void OnInspectorGUI()
        {
            ts.fixedValue = EditorGUILayout.Toggle("FixedValue", ts.fixedValue);
#if !TWEEN_NGUI
            ts.includeChildren = EditorGUILayout.Toggle("InCludeChildren", ts.includeChildren);
#endif

            if (!ts.fixedValue)
            {
                ts.From = EditorGUILayout.FloatField("From", ts.From);
                ts.To = EditorGUILayout.FloatField("To", ts.To);
            }

            base.OnInspectorGUI();
        }


    }

}