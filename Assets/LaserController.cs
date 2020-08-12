using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour {

    private LaserPointer laser;
	// Use this for initialization
	void Start () {
        laser = GetComponent<LaserPointer>();
        laser.laserBeamBehavior = LaserPointer.LaserBeamBehavior.On;
	}

    private void Update()
    {
        RaycastHit hit;
        Physics.Raycast(new Ray(transform.position, transform.forward), out hit);
        laser.SetCursorStartDest(transform.position, hit.point, transform.forward);
    }
}
