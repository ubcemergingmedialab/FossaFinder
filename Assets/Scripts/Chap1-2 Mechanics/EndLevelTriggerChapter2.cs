using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Written by Oliver Vennike Riisager. Fixes the players linerenderer line size depending on which view we are in, also sewaaps the to flyby mode
/// </summary>
public class EndLevelTriggerChapter2 : EndLevelTrigger
{
    [SerializeField]
    Camera flyBy;
    [SerializeField]
    Transform target;
    [SerializeField]
    Canvas main;
    [SerializeField]
    Canvas wanted;
    [SerializeField]
    LineRenderer playerLine;
    [SerializeField]
    GameObject reticle;
    [SerializeField]
    GameObject endScreen;
    private float startWidth;
    private float endWidth;

    void Start()
    {
        startWidth = playerLine.startWidth;
        endWidth = playerLine.endWidth;
    }

    void OnTriggerEnter(Collider collider)
    {
        collider.gameObject.GetComponentInChildren<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll; //Freeze the player
        LevelManager.endGame = true;
        //Show correct UI
        flyBy.gameObject.SetActive(true);
        target.gameObject.SetActive(true);
        main.gameObject.SetActive(false);
        wanted.gameObject.SetActive(true);

        //Fix linesize
        playerLine.startWidth = 19f;
        playerLine.endWidth = 19f;
    }

    /// <summary>
    /// Ends the level, called by the skip button on the ENdScreenCanvas
    /// </summary>
    public void EndLevel()
    {
        LevelManager.endGame = false;
        flyBy.gameObject.SetActive(false);
        target.gameObject.SetActive(false);
        main.gameObject.SetActive(true);
        wanted.gameObject.SetActive(false);

        endLevelEvent.Invoke();//Invoke the endlevel event
   
        //Fix linesize
        playerLine.startWidth = startWidth;
        playerLine.endWidth = endWidth;
        reticle.SetActive(false);
    }


    public void ReturnToCamera()
    {
        LevelManager.endGame = false;
        //Show correct UI
        flyBy.gameObject.SetActive(true);
        target.gameObject.SetActive(true);
        main.gameObject.SetActive(false);
        wanted.gameObject.SetActive(true);

        //Fix linesize
        playerLine.startWidth = 19f;
        playerLine.endWidth = 19f;
    }
}
