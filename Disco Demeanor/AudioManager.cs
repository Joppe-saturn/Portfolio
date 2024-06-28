using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource m_AudioSource;
    private bool playing = false;
    public bool valuesSet = false;

    private void Start()
    {
        m_AudioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if(!m_AudioSource.isPlaying && !playing && valuesSet)
        {
            GetComponent<AudioSource>().Play();
        } else
        {
            playing = true;
        }

        if(playing && !m_AudioSource.isPlaying)
        {
            Destroy(gameObject);
        }
    }
}
