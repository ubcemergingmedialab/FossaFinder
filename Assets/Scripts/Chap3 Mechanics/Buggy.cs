using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Created by Cindy Shi.
/// Destroy the buggy when it collides with the character.
/// </summary>
public class Buggy : MonoBehaviour
{

    private Transform player;
    public Transform particles;
    public ParticleSystem ps;
    public bool em;
    public UIChap3Manager UIMan;
    public AudioChap3Manager audioMan;

    // Start is called before the first frame update
    void Start()
    {
        ParticleSystem ps = particles.GetComponent<ParticleSystem>();
        ps.gameObject.SetActive(false);
        //em = ps.emission.enabled;
        //em = false;
        
    }

  

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            ps.gameObject.SetActive(true);
            //float totalDuration = ps.duration + ps.startLifetime;
            Destroy(this.gameObject,0.5f);
            UIMan.CountUpBuggy();
            audioMan.PlayCollideSound();
        }
    }


    private void OnBecameInvisible()
    {
        Destroy(this.gameObject);

    }
}
