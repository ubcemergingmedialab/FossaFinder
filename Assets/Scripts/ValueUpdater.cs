using UnityEngine;
using UnityEngine.UI;

public class ValueUpdater : MonoBehaviour {
    Text value;
    public GameObject slider;

    // Use this for initialization
    void Start () {
		value = gameObject.GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update () {
        value.text = slider.GetComponent<Slider>().value.ToString();
	}
}
