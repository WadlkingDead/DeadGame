using UnityEngine;
using System.Collections;
using UnityEditor;

namespace LoGameFrame
{

    [CustomEditor(typeof(LoTweenEulerAngle), true)]
    public class LoTweenEulerAngleEditor : LoTweenSingleBaseEditor
    {

        LoTweenEulerAngle tr;

        protected override void OnEnable()
        {
            base.OnEnable();

            tr = (LoTweenEulerAngle)target;

            Undo.IncrementCurrentGroup();
            Undo.SetCurrentGroupName("LoTweenEulerAngle");
        }

        public override void OnInspectorGUI()
        {
            tr.From = EditorGUILayout.Vector3Field("From", tr.From);

            tr.To = EditorGUILayout.Vector3Field("To", tr.To);

            base.OnInspectorGUI();
        }


    }

}
