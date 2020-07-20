using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Created by Cindy Shi.
/// Move the character to where users tap and generate particles when users are tapping.
/// </summary>
public class AgentController : MonoBehaviour
{
    // Start is called before the first frame update
    
    public Camera cam;
    public NavMeshAgent agent;
    public Transform particles;
    public ParticleSystem ps;

    // Start is called before the first frame update
    void Start()
    {
        ParticleSystem ps = particles.GetComponent<ParticleSystem>();
        //ps.gameObject.SetActive(false);

    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit))
            {

                particles.position = hit.point;
                //ps.gameObject.SetActive(true);
                ps.Play();
                agent.SetDestination(hit.point);
            }
        }
    }
}
