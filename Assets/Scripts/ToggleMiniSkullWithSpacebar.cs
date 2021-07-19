using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleMiniSkullWithSpacebar : MonoBehaviour {

    // Use this for initialization
    MiniSkullManager skull;
	void Start () {
        skull = GetComponent<MiniSkullManager>();
	}
	
	// Update is called once per frame
	void Update () {
		if(skull != null && Input.GetKeyDown(KeyCode.Space))
        {
            skull.ToggleMiniSkull();
        }
	}
}
