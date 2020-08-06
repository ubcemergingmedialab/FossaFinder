using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlatformControl : MonoBehaviour
{

    //public NavMeshSurface surface;
    private Vector3 pos1 = new Vector3(-21, 210, 100);
    private Vector3 pos2 = new Vector3(-21, 223, 102);
    public float speed = 0.5f;


    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        //surface.BuildNavMesh();
        //transform.position = Vector3.Lerp(pos1, pos2, Mathf.PingPong(Time.time * speed, 1.0f));
    }
}



