using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Created by Cindy Shi.
/// Display which artery the character is currently on
/// </summary>
public class LandmarkIndicator : MonoBehaviour
{

    public UIChap3Manager UIMan;
    private string arteryName;
    // Start is called before the first frame update
    void Start()
    {
        arteryName = gameObject.name;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
           
            UIMan.DisplayArtery(arteryName);
            Debug.Log("collide");
        }
    }
}
