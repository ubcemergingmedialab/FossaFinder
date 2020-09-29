using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeadRotator : MonoBehaviour {
    bool isRotationButtonPressed;

    // Use this for initialization
    void Start () {
        isRotationButtonPressed = false;
	}
	
	// Update is called once per frame
	void Update () {
        OVRInput.Update();
        if (isRotationButtonPressed)
        {
            Debug.Log(OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick));
            float currentRotationX = Mathf.Clamp(UnityEditor.TransformUtils.GetInspectorRotation(transform).x, -360, 360);
            float currentRotationY = Mathf.Clamp(UnityEditor.TransformUtils.GetInspectorRotation(transform).y, -360, 360);
            float currentRotationZ = Mathf.Clamp(UnityEditor.TransformUtils.GetInspectorRotation(transform).z, -360, 360);
            //float currentrotationy = unityeditor.transformutils.getinspectorrotation(transform).y;
            //float currentrotationz = unityeditor.transformutils.getinspectorrotation(transform).z;
            //if (currentrotationx >= 360)
            //{
            //    currentrotationx -= 360;
            //}
            //if (currentrotationx <= -360)
            //{
            //    currentrotationx += 360;
            //}
            //if (currentrotationy >= 360)
            //{
            //    currentrotationy -= 360;
            //}
            //if (currentrotationy <= -360)
            //{
            //    currentrotationy += 360;
            //}
            //if (currentrotationz >= 360)
            //{
            //    currentrotationz -= 360;
            //}
            //if (currentrotationz <= -360)
            //{
            //    currentrotationz += 360;
            //}
            //Debug.log(currentrotationx);
            UnityEditor.TransformUtils.SetInspectorRotation(transform, new Vector3(currentRotationX + Mathf.RoundToInt(OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).y), currentRotationY + Mathf.RoundToInt(OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).x), currentRotationZ));
        }	
    }

    public void setIsRotationButtonPressed()
    {
        isRotationButtonPressed = !isRotationButtonPressed;
    }

    public void controlAdditionalRotationSettings()
    {
        
    }
}