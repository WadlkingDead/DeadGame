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

    [MenuItem("工具/生成僵尸")]
    static void GenereteZom()
    {
        GameObject obj = GameObject.FindObjectOfType<Nav>().gameObject;
        for (int i = 0; i < 50; i++)
        {
            GameObject go = GameObject.Instantiate(obj);
            go.transform.position = new Vector3(i * 0.5f, 0, i * 0.5f);
        }
    }


}
