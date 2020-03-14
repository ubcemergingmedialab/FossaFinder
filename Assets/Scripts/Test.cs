using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("I have been touched.");
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Plz don't leave");
    }
}
