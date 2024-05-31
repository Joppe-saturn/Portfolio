using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class musicManager : MonoBehaviour
{
    [SerializeField] private List<AudioSource> audioClips = new List<AudioSource>();

    private string musicPlaying;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "Main" && musicPlaying != "Main")
        {
            audioClips[0].Play();
            audioClips[1].Stop();
            musicPlaying = SceneManager.GetActiveScene().name;
        }

        if (SceneManager.GetActiveScene().name == "Title" && musicPlaying != "Title")
        {
            audioClips[1].Play();
            audioClips[0].Stop();
            musicPlaying = SceneManager.GetActiveScene().name;
        }
    }
}
