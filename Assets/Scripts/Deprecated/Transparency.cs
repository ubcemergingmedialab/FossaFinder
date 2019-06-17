using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Required when Using UI elements.

public class Transparency : MonoBehaviour {

    public Slider mainSlider;


    // Use this for initialization
    void Start () {
        Color c = gameObject.GetComponent<Renderer>().material.color;
        c.a = mainSlider.value;
        gameObject.GetComponent<Renderer>().material.color = c;
        mainSlider.onValueChanged.AddListener(delegate { ValueChange(); });
    }

    // Update is called once per frame
    void ValueChange() {
        Color c = gameObject.GetComponent<Renderer>().material.color;
        c.a = mainSlider.value;
        gameObject.GetComponent<Renderer>().material.color = c;
    }
}
