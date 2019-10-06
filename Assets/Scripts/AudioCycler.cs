using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioCycler : MonoBehaviour
{
    public AudioClip[] clips;
    AudioSource source;
    int currentClip;

    void Start()
    {
        source = GetComponent<AudioSource>();
        source.volume = 0.7f;
    }

    // Update is called once per frame
    void Update()
    {
        print(!source.isPlaying);
        if (!source.isPlaying)
        {
            currentClip = (currentClip + 1) % clips.Length;
            source.clip = clips[currentClip];
            source.Play();
        }
        print("index " +currentClip);
    }
}
