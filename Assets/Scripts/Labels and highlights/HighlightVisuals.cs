using System.Collections.Generic;

using UnityEngine;

/*! \brief Manage all the highlights over scences

* 

* This class will manage all highlights on the scene by the number of scene

*/



public class HighlightVisuals : MonoBehaviour

{



    public Dictionary<string, GameObject> availableHighlights;

    public Dictionary<string, Material> HighlightsMaterials;



    public List<GameObject> availableHighlightss;



    /// Material of inactive highlights

    public Material defaultMaterial;

    /// Material of active highlights

    public Material highlightMaterial;



    /*! Setup onEnable

    * 

    * Add EnableHighlights() to eventsystem when enabled

    * @see GuidedTourManager()

    */



    public void OnEnable()

    {

        GuidedTourManager.SetHighlights += EnableHighlights;

        GuidedTourManager.DisableHighlights += DisableAllHighlights;

    }

    /*! Setup OnDisable

    * 

    * Remove EnableHighlights() to eventsystem when enabled

    * @see GuidedTourManager()

    */

    public void OnDisable()

    {

        GuidedTourManager.SetHighlights -= EnableHighlights;

        GuidedTourManager.DisableHighlights -= DisableAllHighlights;

    }



    /*! Initialize list

   * 

   * Add all highlights in the hierarchy to the dictionary

*/

    public void Start()

    {

        //availableHighlights = new Dictionary<string, GameObject>();

        //HighlightsMaterials = new Dictionary<string, Material>();

        //foreach (Transform child in transform)

        //{

        //    availableHighlights.Add(child.name, child.gameObject);

        //    HighlightsMaterials.Add(child.name, child.gameObject.GetComponent<Renderer>().material);



        //}



    }



    /*! \Manage labels on the scene

        * only activate specific highlights by the number of scene

        * @param names Names of fissures that should be shown on the scene

     */



    public void EnableHighlights(string[] names)

    {

        //foreach (KeyValuePair<string, GameObject> pair in availableHighlights)

        //{

        //    pair.Value.GetComponent<Renderer>().material = defaultMaterial;

        //}

        //foreach (string name in names)

        //{

        //    Material rightnow = HighlightsMaterials[name];

        //    print(name);

        //    availableHighlights[name].GetComponent<Renderer>().material = rightnow;

        //}



        foreach (GameObject availableHighlight in availableHighlightss)

        {

            availableHighlight.SetActive(false);

        }



        foreach (string name in names)

        {

            foreach (GameObject availableHighlght in availableHighlightss)

            {

                if (availableHighlght.name == name)

                {

                    availableHighlght.SetActive(true);

                    Debug.Log("HIGHLIGHT: enabling " + availableHighlght.name);

                }

            }

        }

    }



    public void DisableAllHighlights()

    {

        //foreach (KeyValuePair<string, GameObject> pair in availableHighlights)

        //{

        //    pair.Value.GetComponent<Renderer>().material = defaultMaterial;

        //}



        foreach (GameObject availableHighlight in availableHighlightss)

        {

            availableHighlight.SetActive(false);

        }

    }

}