using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GetEyeObject : Editor {

    [MenuItem("工具/选择眼睛")]
    static void SelectEye()
    {
        GameObject obj = GameObject.FindObjectOfType<CameraManager>().gameObject;
        Selection.activeGameObject = obj;
    }


}
