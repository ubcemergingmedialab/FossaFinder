using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Created by Oliver Riisager. Edited by Kimberly Burke.
/// </summary>
public class menuRobert : MonoBehaviour
{
    [SerializeField]
    Camera minimapCam;
    Vector3 dir;
    float speed = 10;
    [SerializeField]
    GameObject left, right, top, bottom;

    Vector3 center;

    // Start is called before the first frame update
    void Start()
    {
        float z = left.transform.position.z;
        dir = new Vector3(UnityEngine.Random.insideUnitSphere.x, UnityEngine.Random.insideUnitSphere.y, 0).normalized;
        //Camera.main.WorldToViewportPoint()
        Vector3 leftBottomCorner = minimapCam.ViewportToWorldPoint(Vector3.zero);
        Vector3 rightTopCorner = minimapCam.ViewportToWorldPoint(new Vector3(1,1,0));

        center = minimapCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
        //left.transform.position = new Vector3(leftBottomCorner.x, center.y, z);
        //right.transform.position = new Vector3(rightTopCorner.x, center.y, z);
        //top.transform.position = new Vector3(center.x, rightTopCorner.y, z);
        //bottom.transform.position = new Vector3(center.x, leftBottomCorner.y, z);
        //minimapCam.targetTexture.height = Screen.height;
        //minimapCam.targetTexture.width = Screen.width;

        // GetComponent<Rigidbody>().AddForce(dir * speed, ForceMode.Impulse);
        GetComponent<Rigidbody>().velocity = new Vector3(1, 1, 1);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        GetComponent<Rigidbody>().AddForce(dir * speed);
    }

    /* private void OnCollisionEnter(Collision collision)
    {
        Vector3 normal = collision.GetContact(0).normal;
        dir = Vector3.Reflect(dir, normal).normalized;
        Debug.Log(dir.normalized);
    } */
}
