using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace LoGameFrame
{
    [CustomEditor(typeof(LoTweenRotation), true)]
    public class LoTweenRotationEditor : LoTweenSingleBaseEditor
    {

        LoTweenRotation tr;

        protected override void OnEnable()
        {
            base.OnEnable();

            tr = (LoTweenRotation)target;

            Undo.IncrementCurrentGroup();
            Undo.SetCurrentGroupName("LoTweenRotation");
        }

        public override void OnInspectorGUI()
        {
            tr.From = Quaternion.Euler(EditorGUILayout.Vector3Field("From", tr.From.eulerAngles));

            tr.To = Quaternion.Euler(EditorGUILayout.Vector3Field("To", tr.To.eulerAngles));

            base.OnInspectorGUI();
        }
    }
}

