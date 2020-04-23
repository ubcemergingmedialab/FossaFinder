using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeadScaler : MonoBehaviour {
    bool isZoomButtonPressed;
    public GameObject smallScaleButton;
    public GameObject mediumScaleButton;
    public GameObject largeScaleButton;

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () { 
	    if (isZoomButtonPressed)
        {
            float verticalValue = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).y;
            float horizontalValue = Mathf.RoundToInt(OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).x);

            transform.localScale = new Vector3(transform.localScale.x + verticalValue/100, transform.localScale.y + verticalValue/100, transform.localScale.z + verticalValue/100);
            // transform.localScale(5);
            // UnityEditor.TransformUtils.S(transform, new Vector3(currentRotationX + Mathf.RoundToInt(OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).y), currentRotationY + Mathf.RoundToInt(OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).x), currentRotationZ));
        }
    }

    public void setIsZoomButtonPressed()
    {
        isZoomButtonPressed = !isZoomButtonPressed;
    }

    public void activateAdditionalZoomSettings()
    {
        if (isZoomButtonPressed)
        {
            smallScaleButton.SetActive(true);
            mediumScaleButton.SetActive(true);
            largeScaleButton.SetActive(true);
        }
        else
        {
            smallScaleButton.SetActive(false);
            mediumScaleButton.SetActive(false);
            largeScaleButton.SetActive(false);
        }
    }

    public void setHeadScale(float scale)
    {
        transform.localScale = new Vector3(scale, scale, scale);
    }
}
