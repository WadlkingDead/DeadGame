using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {

    Camera cam {
        get { return CameraManager.Instance.cam; }
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButton(0))
        {
            Debug.DrawRay(cam.transform.position, new Vector3(0, 0, 2), Color.red);
        }
	}
}
