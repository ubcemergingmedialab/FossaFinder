using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniSkullManager : MonoBehaviour {
    public GameObject skull;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.rotation = skull.transform.rotation;
	}

    public void ToggleMiniSkull()
    {
        gameObject.SetActive(!gameObject.activeInHierarchy);
    }
}
