using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniSkullManager : MonoBehaviour {
    public GameObject skull;
    public GameObject miniSkull;

    private ActivityRecorder recorder;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        miniSkull.transform.rotation = skull.transform.rotation;
        recorder = GameObject.Find("ActivityRecording").GetComponent<ActivityRecorder>();
	}

    public void ToggleMiniSkull()
    {
        miniSkull.SetActive(!miniSkull.activeInHierarchy);
        if(recorder != null)
        {
            recorder.QueueMessage("ToggleMiniSkull");
        }
    }
}
