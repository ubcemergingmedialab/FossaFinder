using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NervesManager : MonoBehaviour {
    public List<GameObject> availableNerves;

    public void OnEnable()
    {
        GuidedTourManager.InitializeEvent += DisableAllNerves;
        GuidedTourManager.VisitPreviousEvent += EnableNerves;
        GuidedTourManager.VisitNextEvent += EnableNerves;
        GuidedTourManager.SkipEvent += EnableNerves;
    }

    public void OnDisable()
    {
        GuidedTourManager.InitializeEvent -= DisableAllNerves;
        GuidedTourManager.VisitPreviousEvent -= EnableNerves;
        GuidedTourManager.VisitNextEvent -= EnableNerves;
        GuidedTourManager.SkipEvent -= EnableNerves;
    }

    public void EnableNerves(SceneData data)
    {
        DisableAllNerves();

        string[] names = data.enabledNerves;
        foreach (string name in names)
        {
            foreach (GameObject availableNerve in availableNerves)
            {
                if (availableNerve.name == name)
                {
                    availableNerve.SetActive(true);
                    //Debug.Log("NERVE: enabling " + availableNerve.name);
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
