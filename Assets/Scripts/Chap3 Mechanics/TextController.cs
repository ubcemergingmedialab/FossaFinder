using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextController : MonoBehaviour
{
    public Camera cam;
    

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        Vector3 targetPostition = new Vector3(cam.transform.position.x,
                                        cam.transform.position.y,
                                        cam.transform.position.z);
        this.transform.LookAt(targetPostition);


      //  transform.LookAt(transform.position + cam.transform.rotation * Vector3.forward, cam.transform.rotation * Vector3.up);
    }
}
