using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Created by Cindy Shi.
/// </summary>
/// 
public class UIChap3Manager : MonoBehaviour
{

    [SerializeField] Text buggyTotalText;
    [SerializeField] Text currentArteryText;
    private int buggyTotal;
    public int buggyCount;
    // Start is called before the first frame update
    void Start()
    {
        buggyTotal = GameObject.FindGameObjectsWithTag("Buggy").Length;
        Debug.Log(buggyTotal);
        buggyCount = 0;
        buggyTotalText.text = buggyCount.ToString() + "/" + buggyTotal.ToString();
        currentArteryText.text = "Vertebral Artery";
    }

    // Update is called once per frame
    void Update()
    {
        //buggyCount = GameObject.FindGameObjectsWithTag("Buggy").Length;
        //Debug.Log(buggyCount);
        
    }

    public void CountUpBuggy()
    {
        buggyCount += 1;
        buggyTotalText.text = buggyCount.ToString() + "/" + buggyTotal.ToString();
    }

    public void DisplayArtery(string arteryName)
    {
        currentArteryText.text = arteryName;
    }

}


