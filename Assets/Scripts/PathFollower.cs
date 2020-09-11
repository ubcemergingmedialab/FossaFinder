using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class PathFollower : MonoBehaviour {
    public PathCreator pathCreator;
    public float speed = 5;
    float distanceTravelled;

	// Use this for initialization
	void Start () {
        Debug.Log(pathCreator.path.localPoints.Length);
	}
	
	// Update is called once per frame
	void Update () {
        distanceTravelled += speed * Time.deltaTime;
        transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled);
    }
}
