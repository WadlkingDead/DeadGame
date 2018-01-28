using UnityEngine;
using System.Collections;
using UnityEditor;

namespace LoGameFrame
{
    public class LoMenuItem : Editor
    {

        [MenuItem("LoGameFrame/查看曲线", false, 900)]
        static void CheckCurves()
        {
            Selection.activeObject = Resources.Load(LoTweenOption.curveFileName);
        }

    }
}


