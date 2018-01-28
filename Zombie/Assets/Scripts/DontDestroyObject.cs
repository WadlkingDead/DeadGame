using UnityEngine;

[DisallowMultipleComponent]
public class DontDestroyObject : MonoBehaviour {

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
	
}
