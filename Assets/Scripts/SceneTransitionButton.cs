using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class SceneTransitionButton : MonoBehaviour {

    public Material defaultColor, activeColor, disabledColor;
    public int targetScene;
    public VRTK_InteractableObject linkedObject;
    private SceneTransitionUI manager;

    // Use this for initialization
    void Start () {
        manager = GetComponentInParent<SceneTransitionUI>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    protected virtual void OnEnable()
    {
        linkedObject = (linkedObject == null ? GetComponent<VRTK_InteractableObject>() : linkedObject);

        if (linkedObject != null)
        {
            linkedObject.InteractableObjectUsed += InteractableObjectUsed;
            linkedObject.InteractableObjectUnused += InteractableObjectUnused;
        }
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

    protected virtual void InteractableObjectUsed(object sender, InteractableObjectEventArgs e)
    {
        Debug.Log("Interactable Object Used");
        manager.ButtonClicked(this.gameObject);
    }

    protected virtual void InteractableObjectUnused(object sender, InteractableObjectEventArgs e)
    {
        Debug.Log("Interacatable Object Unused");
    }

    void OnMouseDown()
    {
        // set this button active color, change current scene
        Debug.Log("button click");
        manager.ButtonClicked(this.gameObject);
    }
}
