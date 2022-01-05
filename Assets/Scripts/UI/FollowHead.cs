using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowHead : MonoBehaviour {

    public float targetDelta = 10f;
    public int angleCount = 50;
    public float torque = 10;
    private Rigidbody rb;
    private int angleCounter = 50;
    private Transform head;
    private bool isAdjusting = false;

    public GameObject headObject;
    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        head = headObject.transform;
        if(rb == null || head == null)
        {
            Debug.LogError("did not find rigidbody on FollowHead script Start");
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void FixedUpdate()
    {
        Vector3 headDirection = head.forward;
        Vector3 UIDirection = transform.forward;
        Vector3 cross;
        float angle = Vector3.Angle(headDirection, UIDirection);
        //Debug.Log("HEAD: " + angle);
        if(angle >= targetDelta)
        {
            headDirection = head.forward;
            cross = Vector3.Cross(transform.forward, headDirection);
            UIDirection = transform.TransformVector(transform.up);
            angle = Vector3.Angle(headDirection, UIDirection);
            angleCounter -= 1;
            //Debug.Log("COUNTER: " + angleCounter);
            if(angleCounter <= 0)
            {
                //Debug.Log("Adding torque: " + headDirection + " " + UIDirection);
                rb.AddTorque(cross * torque);
                isAdjusting = true;
            }
            if (isAdjusting)
            {
                if(angle <= targetDelta)
                {
                    angleCounter = angleCount;
                    isAdjusting = false;
                }
            }
        }
    }
}
