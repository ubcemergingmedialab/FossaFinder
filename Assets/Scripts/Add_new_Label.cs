using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(LabelScript))]
public class Add_new_Label : MonoBehaviour
{
    public LabelScript mytarget;
    //SerializedProperty list;
    //SerializedObject GetTarget;
    bool showWindow = false;
    public Camera cam;
    //public GameObject helpbox;
    private bool mouse;

    public void Start()
    {
        //helpbox.SetActive(false);
    }

    public void Text_Change(string labeltext)
    {
        mytarget.labelText = labeltext;

    }
    public void Dotsize_Change(float dotsize)
    {
        mytarget.dotMul = dotsize;

    }

    public void AddLabel()
    {
        //helpbox.SetActive(true);

        Ray ray = cam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
            //mytarget.ChangeLabelPrefab();
            mytarget.AddLabel(hit.point, hit.normal);
        //helpbox.SetActive(false);
        print(hit.point);


    }
}


