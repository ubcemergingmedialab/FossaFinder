using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRTK;
using VRTK.UnityEventHelper;


// Use: attach to each button on the menu. In inspector, choose the purpose of the button
// Note: TransparencyPresets are used by tranparency buttons and reset. scaleSize is used by scale buttons and reset. part is used by transparency labelling and toggling
public class GenericMenuItemButton : MonoBehaviour {
    public string buttonName;
    public ButtonActions action; //action the button should preform
    public Parts part;      //part to perform it too Note - not used by reset or scale buttons
    public Labels label;    
    public int scaleSize;   
    public float TransparencyPreset1, TransparencyPreset2; //Preset1 for opaque setting, Preset2 for transparent setting
    public Material onMaterial, offMaterial;     //onMaterial for indicating that button is activated, offMaterial for indicating that button is deactivated
    public bool buttonOn;    //bool used in order to change the material of the individual buttons
    public string[] tags;
    private bool state = false;     //used for switching between different transparency preset - indicates if button has been pressed, only used for transparency at the moment
    private VRTK_InteractableObject_UnityEvents buttonEvents;

    void Start()
    {
        buttonEvents = GetComponent<VRTK_InteractableObject_UnityEvents>();
        if (buttonEvents == null)
        {
            buttonEvents = gameObject.AddComponent<VRTK_InteractableObject_UnityEvents>();
        }
        switch (action) //Adds unique listener based on the set button actions
        {
            case ButtonActions.Reset:
                buttonEvents.OnTouch.AddListener(HandleReset);
                break;
            case ButtonActions.Transparency:
                buttonEvents.OnTouch.AddListener(TransparencyListener);
                break;
            case ButtonActions.Scale:
                buttonEvents.OnTouch.AddListener(ScaleListener);
                break;
            case ButtonActions.Toggle:
                buttonEvents.OnTouch.AddListener(ToggleListener);
                break;
            case ButtonActions.Labels:
                buttonEvents.OnTouch.AddListener(LabelListener);
                break;
        }

       // GetComponentInChildren<Text>().text = buttonName;
    }

    void HandleReset(object sender, InteractableObjectEventArgs e)
    {
        ObjectManager.instance.Reset(scaleSize, Parts.skull, TransparencyPreset1, Labels.BonesLabel);
        ObjectManager.instance.Reset(scaleSize, Parts.ppf, TransparencyPreset1, Labels.BonesLabel);
        ObjectManager.instance.Reset(scaleSize, Parts.cavity, TransparencyPreset1, Labels.BonesLabel);
        ObjectManager.instance.Reset(scaleSize, Parts.artery, TransparencyPreset1, Labels.ArteriesLabel);
        ObjectManager.instance.Reset(scaleSize, Parts.nerve, TransparencyPreset1, Labels.NervesLabel);

        //change colors of the buttons to default
        foreach(string tag in tags)
        {
            foreach (GameObject g in GameObject.FindGameObjectsWithTag(tag))
            {
                if(tag != "largeSize" && tag != "default")
                {
                    g.GetComponent<Renderer>().material = offMaterial;
                    g.GetComponent<GenericMenuItemButton>().buttonOn = false;
                }
                else
                {
                    g.GetComponent<Renderer>().material = onMaterial;
                    g.GetComponent<GenericMenuItemButton>().buttonOn = true;
                }
            }
        }
    }

    private void TransparencyListener(object arg0, InteractableObjectEventArgs arg1)
    {
        //calls change alpha on the given part. Alternates between two presets
        ObjectManager.instance.ChangeAlphaOnTag(part, (state ? TransparencyPreset1 : TransparencyPreset2));
        buttonOn = state;
        state = !state;
    }

    private void ScaleListener(object arg0, InteractableObjectEventArgs arg1)
    {
        ObjectManager.instance.ChangeScale(scaleSize);

        switch (gameObject.tag)
        {
            case "lifeSize":
                {
                    GameObject.FindGameObjectWithTag("largeSize").GetComponent<GenericMenuItemButton>().buttonOn = false;
                    GameObject.FindGameObjectWithTag("roomScale").GetComponent<GenericMenuItemButton>().buttonOn = false;
                    break;
                }

            case "largeSize":
                {
                    GameObject.FindGameObjectWithTag("lifeSize").GetComponent<GenericMenuItemButton>().buttonOn = false;
                    GameObject.FindGameObjectWithTag("roomScale").GetComponent<GenericMenuItemButton>().buttonOn = false;
                    break;
                }

            case "roomScale":
                {
                    GameObject.FindGameObjectWithTag("largeSize").GetComponent<GenericMenuItemButton>().buttonOn = false;
                    GameObject.FindGameObjectWithTag("lifeSize").GetComponent<GenericMenuItemButton>().buttonOn = false;
                    break;
                }
        }

        buttonOn = !buttonOn;
    }

    private void ToggleListener(object arg0, InteractableObjectEventArgs arg1)
    {
        ObjectManager.instance.ToggleObjectOnTag(part);
        buttonOn = ObjectManager.instance.IsObjectOn(part);
    }

    private void LabelListener(object arg0, InteractableObjectEventArgs arg1)
    {
        ObjectManager.instance.ToggleLabelOnTag(label, part);
        buttonOn = ObjectManager.instance.IsLabelOn(label);       
    }
     
    void Update()
    {
        GetComponent<Renderer>().material = buttonOn ? onMaterial : offMaterial;
    }
}
