using UnityEngine;
using UnityEngine.UI;
using System;

public class ValueUpdater : MonoBehaviour {
    Text value;
    public GameObject slider;

    // Use this for initialization
    void Start () {
		value = gameObject.GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update () {
        value.text = Math.Round(slider.GetComponent<Slider>().value, 1).ToString(); 
	}
}
