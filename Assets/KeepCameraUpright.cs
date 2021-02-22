using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepCameraUpright : MonoBehaviour {

    public Rigidbody rbMount;
	// Use this for initialization
	void Start () {
        if(rbMount == null) {
            rbMount = GetComponentInParent<Rigidbody>();
        }
    }
	
	// Update is called once per frame
	void FixedUpdate() {
		if(rbMount != null)
        {
            Debug.Log("updating camera rb");
            Quaternion rotationUpright = Quaternion.LookRotation(rbMount.transform.forward, Vector3.up);
            GetComponent<Rigidbody>().MoveRotation(rotationUpright);
        }
	}
}
