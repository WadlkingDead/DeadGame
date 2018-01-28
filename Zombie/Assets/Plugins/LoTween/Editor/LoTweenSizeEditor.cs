using UnityEngine;
using System.Collections;
using UnityEditor;

namespace LoGameFrame
{

    [CustomEditor(typeof(LoTweenSize), true)]
    public class LoTweenSizeEditor : LoTweenSingleBaseEditor
    {

        LoTweenSize ts;

        protected override void OnEnable()
        {
            base.OnEnable();

            ts = (LoTweenSize)target;

            Undo.IncrementCurrentGroup();
            Undo.SetCurrentGroupName("LoTweenSize");
        }

        public override void OnInspectorGUI()
        {

            ts.From = EditorGUILayout.Vector2Field("From", ts.From);
            ts.To = EditorGUILayout.Vector2Field("To", ts.To);

            base.OnInspectorGUI();
        }


    }

}
