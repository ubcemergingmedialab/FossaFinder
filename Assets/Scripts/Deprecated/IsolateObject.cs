using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using VRTK.UnityEventHelper;

public class IsolateObject : MonoBehaviour {

    private GameObject parent;
    public GameObject toIsolate;
    private ArrayList children;
    private bool isIsolate = false;

    private VRTK_Button_UnityEvents buttonEvents;

    private void Start()
    {
        Transform toIsolateTrans = toIsolate.transform;
        parent = toIsolateTrans.parent.gameObject;
        Transform parentTrans = parent.transform;
        children = new ArrayList(parentTrans.childCount);
        for (int i = 0; i < parentTrans.childCount; i++)
        {
            if(parentTrans.GetChild(i).gameObject != toIsolate)
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
        toIsolate.SetActive(true);
        foreach (GameObject obj in children)
        {
            obj.SetActive(isIsolate);
        }
        isIsolate = !isIsolate;
    }
}
