using UnityEngine;
using System.Collections;
using UnityEditor;

namespace LoGameFrame
{

    [CustomEditor(typeof(LoTweenPosition), true)]
    public class LoTweenPositionEditor : LoTweenSingleBaseEditor
    {

        LoTweenPosition tp;

        protected override void OnEnable()
        {
            base.OnEnable();

            tp = (LoTweenPosition)target;

            Undo.IncrementCurrentGroup();
            Undo.SetCurrentGroupName("LoTweenPosition");
        }

        public override void OnInspectorGUI()
        {
            tp.fixedValue = EditorGUILayout.Toggle("FixedValue", tp.fixedValue);
            tp.From = EditorGUILayout.Vector3Field("From", tp.From);
            if (tp.fixedValue)
                tp.Delta = EditorGUILayout.Vector3Field("Delta", tp.Delta);
            else
                tp.To = EditorGUILayout.Vector3Field("To", tp.To);

            tp.changeGlobal = EditorGUILayout.Toggle("Change Global", tp.changeGlobal);

            base.OnInspectorGUI();
        }


    }

}
