using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerController : MonoBehaviour {
    bool isTriggered;
    Renderer render;

	// Use this for initialization
	void Start ()
    {
        isTriggered = false;
        render = GetComponent<Renderer>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter (Collider other)
    {
        if (OVRInput.Get(OVRInput.RawButton.LIndexTrigger))
        {
            if (isTriggered)
            {
                render.material.color = Color.green;
                isTriggered = false;
            } else {
                render.material.color = Color.white;
                isTriggered = true;
            }
            
            
        }
    }
}
