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
        laser.SetCursorRay(transform);
    }
}
