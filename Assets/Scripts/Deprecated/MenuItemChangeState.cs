using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using VRTK.UnityEventHelper;

public class MenuItemChangeState : MonoBehaviour {

    bool isTriggered = false;
    private Material currentState;    

    // Use this for initialization
    void Start()
    {
        currentState = GetComponent<Renderer>().material;
        currentState.color = Color.red;

    }

    void OnTriggerEnter(Collider other)
    {
        if (isTriggered) return;
        isTriggered = true;
        currentState.color = currentState.color == Color.red ? Color.green : Color.red;
    }
    void OnTriggerExit(Collider other)
    {
        isTriggered = false;
        currentState.color = Color.red;
    }
}
