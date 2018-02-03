using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class CameraManager : SingletonMonoBehaviour<CameraManager> {

    [SerializeField]bool _use3D = false;
    public bool USE3D {
        get { return _use3D; }
        set {
            if (_use3D != value)
            {
                _use3D = value;
                Set3DParam(_use3D);
            }
        }
    }

    [SerializeField]float _dis = 0.03f;
    public float distance {
        get { return _dis; }
        set {
            if (_dis != value)
            {
                _dis = Mathf.Clamp(value, 0.001f, 1f);
                leftEye.transform.localPosition = new Vector3(-_dis, 0, 0);
                rightEye.transform.localPosition = new Vector3(_dis, 0, 0);
            }            
        }
    }

    [SerializeField]Camera _left;
    [SerializeField]Camera _right;
    [SerializeField]Camera _middle;

    Camera leftEye
    {
        get {
            if (!_left) _left = transform.Find("Left").GetComponent<Camera>();
            return _left;
        }
    }

    Camera rightEye
    {
        get
        {
            if (!_right) _right = transform.Find("Right").GetComponent<Camera>();
            return _right;
        }
    }

    Camera middleEye
    {
        get
        {
            if (!_middle) _middle = transform.Find("Middle").GetComponent<Camera>();
            return _middle;
        }
    }

    [SerializeField]GameObject _ui;
    GameObject ui {
        get {
            if (!_ui) _ui = GameObject.FindWithTag("UI3D");
            if (!_ui) _ui = GameObject.FindObjectOfType<Canvas>().transform.Find("UIShow3D").gameObject;
            
            return _ui;
        }
    }

    void OnEnable()
    {
        Set3DParam(_use3D);
    }

    void Set3DParam(bool is3d)
    {
        if (!is3d)
        {
            middleEye.gameObject.SetActive(true);
            leftEye.gameObject.SetActive(false);
            rightEye.gameObject.SetActive(false);
            ui.SetActive(false);
        }
        else
        {
            middleEye.gameObject.SetActive(false);
            leftEye.gameObject.SetActive(true);
            rightEye.gameObject.SetActive(true);
            ui.SetActive(true);
        }
    }

    public Camera main {
        get {
            if (USE3D)
                return leftEye;
            else
                return middleEye;
        }
    }



    CameraBrightness[] _darks;
    public CameraBrightness[] darks {
        get {
            if (_darks == null)
            {
                _darks = new CameraBrightness[3];
                _darks[0] = GetAddComponent<CameraBrightness>(leftEye.gameObject);
                _darks[1] = GetAddComponent<CameraBrightness>(middleEye.gameObject);
                _darks[2] = GetAddComponent<CameraBrightness>(rightEye.gameObject);
            }
            return _darks;
        }
    }


    public float darkValue {
        get {
            return darks[0]._Brightness;
        }
        set {
            float target = Mathf.Clamp01(value);
            for (int i = 0; i < 3; i++)
            {
                darks[i]._Brightness = target;
            }
        }
    }

    bool _isDrak;
    public bool isDark {
        get { return _isDrak; }
        set {
            if (_isDrak != value)
            {
                needChange = true;
                _isDrak = value;
            }
        }
    }
    public float beDarkingDuration = 1;
    bool needChange = false;

    public void SetDark()
    {
        Debug.Log("Dark == true");
        isDark = true;
    }
    public void CancelDark()
    {
        Debug.Log("Dark == false");
        isDark = false;
    }



    void Update()
    {
        if (needChange)
        {
            if (isDark)
            {
                darkValue -= Time.deltaTime / beDarkingDuration;
                if (darkValue == 0)
                {
                    needChange = false;
                }
            }
            else
            {
                darkValue += Time.deltaTime / beDarkingDuration;
                if (darkValue == 1)
                {
                    needChange = false;
                }
            }
        }

        
    }




    T GetAddComponent<T>(GameObject go) where T : Component
    {
        T com = go.GetComponent<T>();
        if (!com)
            com = go.AddComponent<T>();

        return com;
    }

}


#if UNITY_EDITOR

[CustomEditor(typeof(CameraManager))]
public class CameraManagerEditor : Editor {

    CameraManager manager;
    void OnEnable()
    {
        manager = target as CameraManager;
    }

    GameObject _city;
    GameObject city {
        get {
            if (!_city)
                _city = GameObject.Find("_City");
            return _city;
        }
    }

    public override void OnInspectorGUI()
    {
        manager.USE3D = EditorGUILayout.Toggle("开启3D", manager.USE3D);
        manager.distance = EditorGUILayout.Slider("景深", manager.distance, 0.001f, 1f);

        manager.beDarkingDuration = EditorGUILayout.FloatField("变黑时长", manager.beDarkingDuration);
        manager.isDark = EditorGUILayout.Toggle("黑屏", manager.isDark);
        if (city)
        {
            bool enabled = city.hideFlags != HideFlags.HideInHierarchy;
            enabled = EditorGUILayout.Toggle("显示City", enabled);
            if (enabled)
                city.hideFlags = HideFlags.None;
            else
                city.hideFlags = HideFlags.HideInHierarchy;
        }
    }


}

#endif