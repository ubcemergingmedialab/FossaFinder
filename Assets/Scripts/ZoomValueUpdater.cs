using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZoomValueUpdater : MonoBehaviour {
    Text zoomValue;
    public GameObject zoomSlider;

	// Use this for initialization
	void Start () {
        zoomValue = gameObject.GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        zoomValue.text = zoomSlider.GetComponent<Slider>().value.ToString();
	}
}
