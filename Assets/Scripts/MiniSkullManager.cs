using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniSkullManager : MonoBehaviour {
    public GameObject skull;
    public GameObject miniSkull;    // 2022 09 05: Adding miniSkill for activation via Spacebar (actual script seems to be missing)

    private ActivityRecorder recorder;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.rotation = skull.transform.rotation;
        recorder = GameObject.Find("ActivityRecording").GetComponent<ActivityRecorder>();

        // 2022 09 05 Daniel Lindenberger: Adding Spacebar to activate MiniSkull here, as I believe that script is missing.
        if (Input.GetKeyDown("space"))
        {
            miniSkull.SetActive(!miniSkull.active);
        }
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
