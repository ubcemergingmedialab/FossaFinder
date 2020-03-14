using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZoomSliderUpdater : MonoBehaviour {
    Slider slider;
    GameObject head;

	// Use this for initialization
	void Start () {
        slider = gameObject.GetComponent<Slider>();
        head = GameObject.Find("Head");
	}
	
	// Update is called once per frame
	void Update () {
        slider.value = head.transform.localScale.x;
	}
}
