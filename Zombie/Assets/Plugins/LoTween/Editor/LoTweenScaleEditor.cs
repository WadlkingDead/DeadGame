using UnityEngine;
using System.Collections;
using UnityEditor;

namespace LoGameFrame
{

    [CustomEditor(typeof(LoTweenScale), true)]
    public class LoTweenScaleEditor : LoTweenSingleBaseEditor
    {

        LoTweenScale ts;

        protected override void OnEnable()
        {
            base.OnEnable();

            ts = (LoTweenScale)target;

            Undo.IncrementCurrentGroup();
            Undo.SetCurrentGroupName("LoTweenScale");
        }

        public override void OnInspectorGUI()
        {
            ts.fixedValue = EditorGUILayout.Toggle("FixedValue", ts.fixedValue);

            if (!ts.fixedValue)
            {
                ts.From = EditorGUILayout.Vector3Field("From", ts.From);
                ts.To = EditorGUILayout.Vector3Field("To", ts.To);
            }

            base.OnInspectorGUI();
        }


    }
}
