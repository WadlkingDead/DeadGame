using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateZom : MonoBehaviour
{
    public GameObject zom;
	// Use this for initialization
	void Start ()
    {
        Generate();
	}
	
	// Update is called once per frame
	void Update () 
    {
		
	}

    void Generate()
    {
        for (int i = 0; i < 50; i++)
        {
            GameObject go = GameObject.Instantiate(zom);
            go.transform.position = new Vector3(i*0.5f, 0, i*0.5f);
        }
    }
}
