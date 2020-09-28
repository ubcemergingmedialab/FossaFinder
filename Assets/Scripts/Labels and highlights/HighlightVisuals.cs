using System.Collections.Generic;
using UnityEngine;

/*! \brief Manage all the highlights over scences
* 
* This class will manage all highlights on the scene by the number of scene
*/



public class HighlightVisuals : MonoBehaviour
{
    public Dictionary<string, Material> highlightsMaterials;
    public List<GameObject> availableHighlights;

    /// Material of inactive highlights
    public Material defaultMaterial;

    /// Material of active highlights
    public Material highlightMaterial;



    /*! Setup onEnable
    * 
    * Add EnableHighlights() to eventsystem when enabled
    * @see GuidedTourManager
    */



    public void OnEnable()
    {
        GuidedTourManager.SkipEvent += EnableHighlights;
        GuidedTourManager.VisitPreviousEvent += EnableHighlights;
        GuidedTourManager.VisitNextEvent += EnableHighlights;
        GuidedTourManager.ZoomInEvent += EnableHighlights;
        GuidedTourManager.InitializeEvent += DisableAllHighlights;
    }

    /*! Setup OnDisable
    * 
    * Remove EnableHighlights() to eventsystem when enabled
    * @see GuidedTourManager()
    */

    public void OnDisable()
    {
        GuidedTourManager.SkipEvent -= EnableHighlights;
        GuidedTourManager.VisitPreviousEvent -= EnableHighlights;
        GuidedTourManager.VisitNextEvent -= EnableHighlights;
        GuidedTourManager.ZoomInEvent -= EnableHighlights;
        GuidedTourManager.InitializeEvent -= DisableAllHighlights;
    }
    
    /*! \Manage labels on the scene
        * only activate specific highlights by the number of scene
        * @param names Names of fissures that should be shown on the scene
     */
    public void EnableHighlights(SceneData data)
    {
        DisableAllHighlights();
        string[] names = data.highlights;
        foreach (string name in names)
        {
            foreach (GameObject availableHighlght in availableHighlights)
            {
                if(availableHighlght.name == name)
                {
                    availableHighlght.SetActive(true);
                    Debug.Log("HIGHLIGHT: enabling " + availableHighlght.name);
                }
            }
        }
    }

    public void DisableAllHighlights()
    {
        foreach (GameObject availableHighlight in availableHighlights)
        {
            availableHighlight.SetActive(false);
        }
    }
}