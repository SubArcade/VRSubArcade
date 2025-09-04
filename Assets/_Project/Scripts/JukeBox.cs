using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JukeBox : MonoBehaviour
{
    public AudioClip[] bgms;
    public AudioSource Audio;

    private int currentIndex = 0;


    void Start()
    {
        Audio = GetComponent<AudioSource>();
        SongStart();
    }

    public void SongStart() 
    {
        if (bgms.Length == 0) return;

        Audio.clip = bgms[currentIndex];
        Audio.Play();
    }

    public void SongStop() 
    {
        Audio.Stop();
    }

    public void SongChange() 
    {
        if (bgms.Length == 0) return ;

        currentIndex = Random.Range(0, bgms.Length);
        Audio.clip = bgms[currentIndex];
        Audio.Play();
    }
}
