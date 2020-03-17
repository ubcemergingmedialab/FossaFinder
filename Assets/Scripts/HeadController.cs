using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HeadController : MonoBehaviour {
    Rigidbody rigidbody;
    float rawInputFactor;
   
    public GameObject xPositionSlider;
    public GameObject yPositionSlider;
    public GameObject zPositionSlider;
    Vector3 defaultHeadPosition;
    Vector3 leftControllerPosition;
    float timeElapsed;
    float speed;
    float journeyLength;
    bool journeyIsSet;
    float fractionOfJourney;

    public GameObject smallScaleButton;
    public GameObject mediumScaleButton;
    public GameObject largeScaleButton;

	// Use this for initialization
	void Start () {
        rigidbody = gameObject.GetComponent<Rigidbody>();
        rawInputFactor = 100f;

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
        OVRInput.Update();

		if (transform.parent == null)
        {
            // rigidbody.Sleep();
            rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        } else
        {
            // rigidbody.WakeUp();
            rigidbody.constraints = RigidbodyConstraints.None;
        }

        if (transform.parent != null && OVRInput.Get(OVRInput.Button.PrimaryThumbstick))
        {
            // Debug.Log("Are we here yet?");
            transform.position += new Vector3(OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).x / rawInputFactor, OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).y / rawInputFactor, 0);

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

    public void setDefaultTransform()
    {

    }

    public void setHeadPositionX(float rotationValue)
    {
        transform.position = new Vector3(rotationValue, transform.position.y, transform.position.z);
    }

    public void incrHeadPositionX()
    {
        float currentPositionX = Mathf.Clamp(transform.position.x + 1, -10, 10);
        transform.position = new Vector3(currentPositionX, transform.position.y, transform.position.z);
    }

    public void decrHeadPositionX()
    {
        float currentPositionX = Mathf.Clamp(transform.position.x - 1, -10, 10);
        transform.position = new Vector3(currentPositionX, transform.position.y, transform.position.z);
    }

    public void setHeadPositionY(float rotationValue)
    {
        transform.position = new Vector3(transform.position.x, rotationValue, transform.position.z);
    }

    public void incrHeadPositionY()
    {
        float currentPositionY = Mathf.Clamp(transform.position.y + 1, 0, 20);
        transform.position = new Vector3(transform.position.x, currentPositionY, transform.position.z);
    }

    public void decrHeadPositionY()
    {
        float currentPositionY = Mathf.Clamp(transform.position.y + 1, 0, 20);
        transform.position = new Vector3(transform.position.x, currentPositionY, transform.position.z);
    }

    public void setHeadPositionZ(float rotationValue)
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, rotationValue);
    }

    public void incrHeadPositionZ()
    {
        float currentPositionZ = Mathf.Clamp(transform.position.z + 1, -10, 10);
        transform.position = new Vector3(transform.position.x, transform.position.y, currentPositionZ);
    }

    public void decrHeadPositionZ()
    {
        float currentPositionZ = Mathf.Clamp(transform.position.z - 1, -10, 10);
        transform.position = new Vector3(transform.position.x, transform.position.y, currentPositionZ);
    }

    public void setHeadRotationX (float rotationValue)
    {
        UnityEditor.TransformUtils.SetInspectorRotation(transform, new Vector3(rotationValue, UnityEditor.TransformUtils.GetInspectorRotation(transform).y, UnityEditor.TransformUtils.GetInspectorRotation(transform).z));
    }

    public void incrHeadRotationX()
    {
        float currentRotationX = Mathf.Clamp(UnityEditor.TransformUtils.GetInspectorRotation(transform).x + 1, -360, 360);
        UnityEditor.TransformUtils.SetInspectorRotation(transform, new Vector3(currentRotationX, UnityEditor.TransformUtils.GetInspectorRotation(transform).y, UnityEditor.TransformUtils.GetInspectorRotation(transform).z));
    }

    public void decrHeadRotationX()
    {
        float currentRotationX = Mathf.Clamp(UnityEditor.TransformUtils.GetInspectorRotation(transform).x - 1, -360, 360);
        UnityEditor.TransformUtils.SetInspectorRotation(transform, new Vector3(currentRotationX, UnityEditor.TransformUtils.GetInspectorRotation(transform).y, UnityEditor.TransformUtils.GetInspectorRotation(transform).z));
    }

    public void setHeadRotationY(float rotationValue)
    {
        UnityEditor.TransformUtils.SetInspectorRotation(transform, new Vector3(UnityEditor.TransformUtils.GetInspectorRotation(transform).x, rotationValue, UnityEditor.TransformUtils.GetInspectorRotation(transform).z));
    }

    public void incrHeadRotationY()
    {
        float currentRotationY = Mathf.Clamp(UnityEditor.TransformUtils.GetInspectorRotation(transform).y + 1, -360, 360);
        UnityEditor.TransformUtils.SetInspectorRotation(transform, new Vector3(UnityEditor.TransformUtils.GetInspectorRotation(transform).x, currentRotationY, UnityEditor.TransformUtils.GetInspectorRotation(transform).z));
    }

    public void decrHeadRotationY()
    {
        float currentRotationY = Mathf.Clamp(UnityEditor.TransformUtils.GetInspectorRotation(transform).y - 1, -360, 360);
        UnityEditor.TransformUtils.SetInspectorRotation(transform, new Vector3(UnityEditor.TransformUtils.GetInspectorRotation(transform).x, currentRotationY, UnityEditor.TransformUtils.GetInspectorRotation(transform).z));
    }

    public void setHeadRotationZ(float rotationValue)
    {
        UnityEditor.TransformUtils.SetInspectorRotation(transform, new Vector3(UnityEditor.TransformUtils.GetInspectorRotation(transform).x, UnityEditor.TransformUtils.GetInspectorRotation(transform).y, rotationValue));
    }

    public void incrHeadRotationZ()
    {
        float currentRotationZ = Mathf.Clamp(UnityEditor.TransformUtils.GetInspectorRotation(transform).z + 1, -360, 360);
        UnityEditor.TransformUtils.SetInspectorRotation(transform, new Vector3(UnityEditor.TransformUtils.GetInspectorRotation(transform).y, UnityEditor.TransformUtils.GetInspectorRotation(transform).y, currentRotationZ));
    }

    public void decrHeadRotationZ()
    {
        float currentRotationZ = Mathf.Clamp(UnityEditor.TransformUtils.GetInspectorRotation(transform).z - 1, -360, 360);
        UnityEditor.TransformUtils.SetInspectorRotation(transform, new Vector3(UnityEditor.TransformUtils.GetInspectorRotation(transform).y, UnityEditor.TransformUtils.GetInspectorRotation(transform).y, currentRotationZ));
    }

    public void setHeadScale(float scale)
    {
        transform.localScale = new Vector3(scale, scale, scale);
    }

    public void incrHeadScale()
    {
        transform.localScale = new Vector3(transform.localScale.x + 1, transform.localScale.y + 1, transform.localScale.z + 1);
    }

    public void decrHeadScale()
    {
        transform.localScale = new Vector3(transform.localScale.x - 1, transform.localScale.y - 1, transform.localScale.z - 1);
    }
}
