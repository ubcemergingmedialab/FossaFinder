using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniSkullManager : MonoBehaviour {
    public GameObject skull;

    private ActivityRecorder recorder;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.rotation = skull.transform.rotation;
        //recorder = GameObject.Find("ActivityRecording").GetComponent<ActivityRecorder>();
	}

    public void ToggleMiniSkull()
    {
        gameObject.SetActive(!gameObject.activeInHierarchy);
        if(recorder != null)
        {
            recorder.QueueMessage("ToggleMiniSkull");
        }
    }
}
