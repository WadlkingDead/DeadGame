using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class HideInHierarchy : MonoBehaviour {

    void Awake()
    {
        gameObject.hideFlags = HideFlags.HideInHierarchy;
    }

}
