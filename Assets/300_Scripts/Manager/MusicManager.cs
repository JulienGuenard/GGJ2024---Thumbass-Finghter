using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioClip music;

    public AudioSource audioS_Music;
    public AudioSource audioS_SFX;

    public static MusicManager instance;

    void Awake()
    {
        audioS_Music = GetComponent<AudioSource>();

        if (instance == null) instance = this;
    }

    private void Start()
    {
        audioS_Music.PlayOneShot(music);
    }
}
