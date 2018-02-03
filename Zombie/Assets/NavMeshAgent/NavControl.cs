using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavControl : MonoBehaviour {

    Nav[] zombies;
    void Awake()
    {
        zombies = FindObjectsOfType<Nav>();
    
    }

    public void OnEnable()
    {
        float ran;
        for (int i = 0; i < zombies.Length; i++)
        {
            ran = Random.Range(0f, 3f);
            if(ran < 1) 
                zombies[i].Born();
            else if(ran<2)
                zombies[i].PlayIdel();
            else
                zombies[i].PlayWalk();
        }
    }

}
