using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabelListener : MonoBehaviour {

    private static LabelListener _instance;
    public static LabelListener Instance
    {
        get { return _instance; }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
}
