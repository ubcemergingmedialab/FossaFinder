using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/*! \brief Manage all the labels over scences
* 
* This class will disable all labels on the scene when the scene transformed
*/

public class LabelManager : MonoBehaviour {
    public List<GameObject> availableLabels;

    /*! Setup onEnable
    * 
    * Add DisbleLabels() to eventsystem when enabled
    * @see GuidedTourManager()
    */
    public void OnEnable()
    {
        GuidedTourManager.InitializeEvent += DisableLabels;
        GuidedTourManager.VisitPreviousEvent += DisableLabels;
        GuidedTourManager.VisitNextEvent += DisableLabels;
        GuidedTourManager.ZoomOutEvent += DisableLabels;
        GuidedTourManager.SkipEvent += DisableLabels;
    }

    /*! Setup OnDisable
    * 
    * Remove DisbleLabels() to eventsystem when enabled
    * @see GuidedTourManager()
    */
    void OnDisable()
    {
        GuidedTourManager.InitializeEvent -= DisableLabels;
        GuidedTourManager.VisitPreviousEvent -= DisableLabels;
        GuidedTourManager.VisitNextEvent -= DisableLabels;
        GuidedTourManager.ZoomOutEvent -= DisableLabels;
        GuidedTourManager.SkipEvent -= DisableLabels;
    }

     /*! \Disable labels on the scene
        * 
        * 
     */
    public void DisableLabels()
    {
        foreach (GameObject availableLabel in availableLabels)
        {
            availableLabel.SetActive(false);
        }
    }

    public void DisableLabels(SceneData sceneData)
    {
        DisableLabels();
    }
}
