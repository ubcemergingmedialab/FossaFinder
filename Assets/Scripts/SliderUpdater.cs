using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderUpdater : MonoBehaviour {
    public GameObject XPositionSlider;
    public GameObject YPositionSlider;
    public GameObject ZPositionSlider;
    public GameObject XRotationSlider;
    public GameObject YRotationSlider;
    public GameObject ZRotationSlider;
    public GameObject ZoomSlider;

    Slider thisSlider;
    string thisSliderName;
    GameObject head;

    // Use this for initialization
    void Start () {
        thisSlider = gameObject.GetComponent<Slider>();
        thisSliderName = thisSlider.transform.name;
        head = GameObject.Find("Head");
	}
	
	// Update is called once per frame
	void Update () {
        if (thisSliderName == XPositionSlider.transform.name)
        {
            thisSlider.value = head.transform.position.x;
        } else if (thisSliderName == YPositionSlider.transform.name)
        {
            thisSlider.value = head.transform.position.y;
        } else if (thisSliderName == ZPositionSlider.transform.name)
        {
            thisSlider.value = head.transform.position.z;
        } else if (thisSliderName == XRotationSlider.transform.name)
        {
            thisSlider.value = UnityEditor.TransformUtils.GetInspectorRotation(head.transform).x;
            // thisSlider.value = head.transform.rotation.eulerAngles.x;
        } else if (thisSliderName == YRotationSlider.transform.name)
        {
            thisSlider.value = UnityEditor.TransformUtils.GetInspectorRotation(head.transform).y;
            // thisSlider.value = head.transform.rotation.eulerAngles.y;
        }
        else if (thisSliderName == ZRotationSlider.transform.name)
        {
            thisSlider.value = UnityEditor.TransformUtils.GetInspectorRotation(head.transform).z;
            // thisSlider.value = head.transform.rotation.eulerAngles.z;
        } else if (thisSliderName == ZoomSlider.transform.name)
        {
            thisSlider.value = thisSlider.value = head.transform.localScale.x;
        }        
	}
}
