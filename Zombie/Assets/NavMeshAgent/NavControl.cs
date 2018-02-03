using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavControl : MonoBehaviour {

    Nav[] zombies;
    void Start()
    {
        zombies = FindObjectsOfType<Nav>();
    
    }

    public void Born()
    {
        for (int i = 0; i < zombies.Length; i++)
        {
            zombies[i].Born();
        }
    }

}
