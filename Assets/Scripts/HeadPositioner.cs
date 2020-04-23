using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HeadPositioner : MonoBehaviour {
    public GameObject defaultPositionButton;
    public GameObject xPositionSlider;
    public GameObject yPositionSlider;
    public GameObject zPositionSlider;

    bool isPositionButtonPressed;

    Vector3 defaultHeadPosition;
    Vector3 leftControllerPosition;
    float timeElapsed;
    float speed;
    float journeyLength;
    bool journeyIsSet;
    float fractionOfJourney;
    
    // Use this for initialization
    void Start () {
        isPositionButtonPressed = false;

        defaultHeadPosition = GameObject.Find("Head").transform.position;
        leftControllerPosition = new Vector3(0, 0, 0);
        timeElapsed = 0f;
        speed = 1f;
        journeyLength = Vector3.Distance(defaultHeadPosition, leftControllerPosition);

        journeyIsSet = false;
        fractionOfJourney = 0f;
	}
	
	// Update is called once per frame
	void Update () {
        if (true)
        {
            transform.position += new Vector3(OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).x/100, OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).y/100, 0);

            if (OVRInput.Get(OVRInput.Button.PrimaryThumbstick))
            {
                if (!journeyIsSet)
                {
                    timeElapsed = 0f;
                    defaultHeadPosition = GameObject.Find("Head").transform.position;
                    leftControllerPosition = InputTracking.GetLocalPosition(XRNode.LeftHand);
                    journeyLength = Vector3.Distance(defaultHeadPosition, leftControllerPosition);

                    journeyIsSet = true;
                }

                timeElapsed += Time.deltaTime;
                float distCovered = timeElapsed * speed;
                fractionOfJourney = distCovered / journeyLength;
                transform.position = Vector3.Lerp(defaultHeadPosition, leftControllerPosition, fractionOfJourney);

                if (fractionOfJourney >= 0.999f) // can set additional conditions here to reset, such as triggering other buttons
                {
                    timeElapsed = 0f;
                    journeyIsSet = false;
                }
            }
        }
    }

    public void setIsPositionButtonPressed()
    {
        isPositionButtonPressed = !isPositionButtonPressed;
    }

    public void activateAdditionalPositionSettings()
    {
        if (isPositionButtonPressed)
        {
            defaultPositionButton.SetActive(true);
            xPositionSlider.SetActive(true);
            yPositionSlider.SetActive(true);
            zPositionSlider.SetActive(true);
        } else
        {
            defaultPositionButton.SetActive(false);
            xPositionSlider.SetActive(false);
            yPositionSlider.SetActive(false);
            zPositionSlider.SetActive(false);
        }
    }
}
