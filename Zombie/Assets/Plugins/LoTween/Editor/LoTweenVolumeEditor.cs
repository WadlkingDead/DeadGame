using UnityEngine;
using System.Collections;
using UnityEditor;

namespace LoGameFrame
{
    [CustomEditor(typeof(LoTweenVolume), true)]
    public class LoTweenVolumeEditor : LoTweenSingleBaseEditor
    {

        LoTweenVolume tv;

        protected override void OnEnable()
        {
            base.OnEnable();

            tv = (LoTweenVolume)target;

            Undo.IncrementCurrentGroup();
            Undo.SetCurrentGroupName("LoTweenVolume");
        }

        public override void OnInspectorGUI()
        {
            tv.fixedValue = EditorGUILayout.Toggle("FixedValue", tv.fixedValue);

            if (!tv.fixedValue)
            {
                tv.From = EditorGUILayout.FloatField("From", tv.From);
                tv.To = EditorGUILayout.FloatField("To", tv.To);
            }

            base.OnInspectorGUI();
        }


    }
}