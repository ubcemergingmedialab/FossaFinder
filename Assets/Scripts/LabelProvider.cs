using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRTK;

public class LabelProvider : MonoBehaviour {

    public string labelText;
    public VRTK_InteractableObject linkedObject;
	// Use this for initialization
	void Start () {

        linkedObject = (linkedObject == null ? GetComponent<VRTK_InteractableObject>() : linkedObject);

        if (linkedObject != null)
        {
            linkedObject.InteractableObjectUsed += InteractableObjectUsed;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnMouseDown()
    {
        Debug.Log("something was clicked");
        LabelListener.Instance.ChangeText(labelText);

    }

    protected virtual void InteractableObjectUsed(object sender, InteractableObjectEventArgs e)
    {
        LabelListener.Instance.ChangeText(labelText);
    }
}
