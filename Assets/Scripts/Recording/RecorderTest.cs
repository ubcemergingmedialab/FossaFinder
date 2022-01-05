using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecorderTest : MonoBehaviour {

    ActivityRecorder recorder;
	// Use this for initialization
	void Start () {
        recorder = GetComponent<ActivityRecorder>();
	}
	
	// Update is called once per frame
	void Update () {
        recorder.QueueMessage("Test");
	}
}
