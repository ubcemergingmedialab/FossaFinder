using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NervesManager : MonoBehaviour {
    public List<GameObject> availableNerves;

    public void OnEnable()
    {
        GuidedTourManager.SetNerves += EnableNerves;
        GuidedTourManager.DisableNerves += DisableAllNerves;
    }

    public void OnDisable()
    {
        GuidedTourManager.SetNerves -= EnableNerves;
        GuidedTourManager.DisableNerves -= DisableAllNerves;
    }

    public void EnableNerves(string[] names)
    {
        foreach (GameObject availableNerve in availableNerves)
        {
            availableNerve.SetActive(false);
        }

        foreach (string name in names)
        {
            foreach (GameObject availableNerve in availableNerves)
            {
                if (availableNerve.name == name)
                {
                    availableNerve.SetActive(true);
                    Debug.Log("NERVE: enabling " + availableNerve.name);
                }
            }
        }
    }

    public void DisableAllNerves()
    {
        foreach (GameObject availableNerve in availableNerves)
        {
            availableNerve.SetActive(false);
        }
    }
}
