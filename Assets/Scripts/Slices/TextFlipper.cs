using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Writteb by Oliver Vennike Riisager- used to flip text and make it face the camera.
/// </summary>
public class TextFlipper : MonoBehaviour
{
    private static GameObject endCam;

    // Start is called before the first frame update
    void Start()
    {
        if (ObjectiveUtility.IsMotor)
        {
            transform.position = new Vector3(-transform.position.x, transform.position.y, transform.position.z);
            transform.rotation = Quaternion.Euler(-180, 0, -180);
        }
    }

    private void LateUpdate()
    {
        if (LevelManager.endGame)
        {
            if (endCam == null)
                endCam = GameObject.Find("FlyByCamera");
            transform.LookAt(endCam.transform, endCam.transform.up);
            transform.rotation = transform.rotation * Quaternion.Euler(0, -180, 0);
        }
    }
}
