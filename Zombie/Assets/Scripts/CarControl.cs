using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarControl : MonoBehaviour {

    public enum CtrlMode {
        Self, Auto,
    }

    public CtrlMode ctrlMode = CtrlMode.Self;

    Rigidbody rig;

	// Use this for initialization
	void Start () {
        rig = GetComponent<Rigidbody>();
	}

    Vector3 targetPos;
    Vector3 targetRot;

	// Update is called once per frame
	void Update () {
        targetPos = transform.localPosition;
        targetRot = transform.localEulerAngles;
        if (ctrlMode == CtrlMode.Self)
        {

            targetPos += transform.forward * Input.GetAxis("Vertical");
            targetRot += transform.up * Input.GetAxis("Horizontal") * 15f;
        }
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, 0.6f);
        transform.localEulerAngles = Vector3.Lerp(transform.localEulerAngles, targetRot, 0.6f);
	}
}
