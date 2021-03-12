using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LabelProvider : MonoBehaviour {

    public string labelText;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnMouseDown()
    {
        Debug.Log("something was clicked");
        LabelListener.Instance.ChangeText(labelText);

    }
}
