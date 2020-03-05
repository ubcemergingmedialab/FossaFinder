using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotate : MonoBehaviour {
    Quaternion m_Rotation = Quaternion.identity;
    Rigidbody m_Rigidbody;
    // Use this for initialization
    void Start () {
        m_Rigidbody = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        float h =  Input.GetAxis("Mouse X")* 60.0f * Time.deltaTime;
        float v =  Input.GetAxis("Mouse Y") * 60.0f * Time.deltaTime;
 transform.Rotate(v, h, 0);
        //transform.Rotate(Vector3.up * 20.0f * Time.deltaTime);
    }
}
