using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompassManager : MonoBehaviour {
    public GameObject pointer;
    public GameObject entrance;

    Vector3 pointOfIntersection;
    float radius;
    float offset;

	// Use this for initialization
	void Start () {
        offset = 0.01f;
	}
	
	// Update is called once per frame
	void Update () {
        radius = GetComponent<SphereCollider>().radius;
        pointer.transform.position = transform.position + (radius + offset) * (entrance.transform.position - transform.position) / Vector3.Magnitude(entrance.transform.position - transform.position);
        pointer.transform.LookAt(entrance.transform, Vector3.left);
	}
}
