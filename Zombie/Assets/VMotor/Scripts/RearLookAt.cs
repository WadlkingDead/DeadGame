using UnityEngine;
using System.Collections;

public class RearLookAt : MonoBehaviour
{


    public Transform target;

	void Update () {

        Vector3 dist = transform.localPosition - target.localPosition;
        Quaternion rot = Quaternion.LookRotation(dist);
        transform.localRotation = rot;
	}
}
