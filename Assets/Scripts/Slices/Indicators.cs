using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indicators : MonoBehaviour
{
    [SerializeField]
    Material correct;
    [SerializeField]
    Material wrong;
    private static GameObject endCam;
    private Renderer renderer;
    Material current;

    // Start is called before the first frame update
    void Start()
    {
        if (endCam == null)
            endCam = GameObject.Find("FlyByCamera");

        if (ObjectiveUtility.IsMotor)
            transform.position = new Vector3(-transform.position.x, transform.position.y, transform.position.z); //If motor, flip our x
    }
    /// <summary>
    ///  Sets the indicator to display positive texture. Called from LevelManager.
    /// </summary>
    public void SetPostive()
    {
        if (renderer == null)
            renderer = GetComponent<Renderer>();
        renderer.material = correct;
    }

    /// <summary>
    ///  Sets the indicator to display negative texture. Called from LevelManager.
    /// </summary>
    public void SetNegative()
    {
        if (renderer == null)
            renderer = GetComponent<Renderer>();
        renderer.material = wrong;
    }

    private void LateUpdate()
    {
        Vector3 dir = (endCam.transform.position - transform.position).normalized;//Since the indicators are flipped weirdly, we have to manually make them look at the camera
        Quaternion lookRotation = Quaternion.LookRotation(transform.forward, dir);//Forward acts as its "up" direction due to the way it is facing. Then the forward will be the directional vector towards the camera. 

        transform.rotation = lookRotation;
    }
}
