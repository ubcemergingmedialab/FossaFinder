using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRTK;

public class LabelProvider : MonoBehaviour {

    public string labelText;
    public VRTK_InteractableObject linkedObject;
    private MeshRenderer renderer;
    private Light light;
    private Color color;
	// Use this for initialization
	void Start () {

        linkedObject = (linkedObject == null ? GetComponent<VRTK_InteractableObject>() : linkedObject);

        if (linkedObject != null)
        {
            linkedObject.InteractableObjectUsed += InteractableObjectUsed;
        }
        renderer = GetComponent<MeshRenderer>();
        if (renderer == null)
        {
            light = GetComponentInParent<Light>();
            color = light.color;
        }
        else
        {
            color = renderer.material.color;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnMouseDown()
    {
        Debug.Log("something was clicked");
        LabelListener.Instance.ChangeText(labelText);
        LabelListener.Instance.ChangeColor(color);

    }

    protected virtual void InteractableObjectUsed(object sender, InteractableObjectEventArgs e)
    {
        LabelListener.Instance.ChangeText(labelText);
        LabelListener.Instance.ChangeColor(color);
    }
}
