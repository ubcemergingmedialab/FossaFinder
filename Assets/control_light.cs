using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class control_light : MonoBehaviour {
	public GameObject refer;
	public GameObject thislight;
	

	// Use this for initialization
	void Start () {
		thislight.SetActive(false);
	}



	// Update is called once per frame
	void Update () {
		if (refer.GetComponent<Text>().text == "9") {
			thislight.SetActive(true);
					}
	}
}
