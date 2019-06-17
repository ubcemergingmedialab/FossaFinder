using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using VRTK.UnityEventHelper;

public class ResetButtonBehaviour : MonoBehaviour {

    public GameObject parent;
    private ArrayList children;

    private VRTK_Button_UnityEvents buttonEvents;

    private void Start()
    {
        Transform parentTrans = parent.transform;
        children = new ArrayList(parentTrans.childCount);
        for (int i = 0; i < parentTrans.childCount; i++)
        {
            children.Add(parentTrans.GetChild(i).gameObject);
        }
        buttonEvents = GetComponent<VRTK_Button_UnityEvents>();
        if (buttonEvents == null)
        {
            buttonEvents = gameObject.AddComponent<VRTK_Button_UnityEvents>();
        }
        buttonEvents.OnPushed.AddListener(handlePush);
    }

    private void handlePush(object sender, Control3DEventArgs e)
    {
        foreach(GameObject obj in children)
        {
            obj.SetActive(true);
        }
    }
}
