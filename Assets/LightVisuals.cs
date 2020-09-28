using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightVisuals : MonoBehaviour {
    public List<GameObject> availableLights;

    // Use this for initialization

    /*! Setup onEnable
* 
* Add Enablelights() to eventsystem when enabled
* @see GuidedTourManager()
*/

    public void OnEnable()
    {
        GuidedTourManager.InitializeEvent += DisableAllLights;
        GuidedTourManager.ZoomOutEvent += DisableAllLights;
        GuidedTourManager.VisitPreviousEvent += EnableLights;
        GuidedTourManager.VisitNextEvent += EnableLights;
        GuidedTourManager.ZoomInEvent += EnableLights;
        GuidedTourManager.SkipEvent += EnableLights;
    }

    /*! Setup OnDisable
    * 
    * Remove Enablelights() to eventsystem when enabled
    * @see GuidedTourManager()
    */
    public void OnDisable()
    {
        GuidedTourManager.InitializeEvent -= DisableAllLights;
        GuidedTourManager.ZoomOutEvent -= DisableAllLights;
        GuidedTourManager.VisitPreviousEvent -= EnableLights;
        GuidedTourManager.VisitNextEvent -= EnableLights;
        GuidedTourManager.ZoomInEvent -= EnableLights;
        GuidedTourManager.SkipEvent -= EnableLights;
    }

    /*! \Manage lightss on the scene
        * only activate specific lights by the number of scene
        * @param names Names of fissures that should be shown on the scene
     */

    public void EnableLights(SceneData sceneData)
    {
        string[] names = sceneData.lights;
        foreach (GameObject availableLight in availableLights)
        {
            availableLight.SetActive(false);
        }

        foreach (string name in names)
        {
            foreach (GameObject availableLight in availableLights)
            {
                if (availableLight.name == name)
                {
                    availableLight.SetActive(true);
                    Debug.Log("LIGHT: enabling " + availableLight.name);
                }
            }
        }
    }

    public void DisableAllLights()
    {
        foreach (GameObject availableLight in availableLights)
        {
            availableLight.SetActive(false);
        }
    }
    public void DisableAllLights(SceneData sceneData)
    {
        DisableAllLights();
    }
}
