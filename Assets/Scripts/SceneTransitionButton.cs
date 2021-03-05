using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SceneTransitionButton : MonoBehaviour {

    public Material defaultColor, activeColor, disabledColor;
    public int targetScene;
    private SceneTransitionUI manager;

    // Use this for initialization
    void Start () {
        manager = GetComponentInParent<SceneTransitionUI>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetDefaultColor()
    {
        GetComponent<MeshRenderer>().material = defaultColor;
    }

    public void SetActiveColor()
    {
        GetComponent<MeshRenderer>().material = activeColor;
    }

    public void SetDisabledColor()
    {
        GetComponent<MeshRenderer>().material = disabledColor;
    }

    void OnMouseDown()
    {
        // set this button active color, change current scene
        //Debug.Log("button clicked");
        manager.ButtonClicked(this.gameObject);

    }
}
