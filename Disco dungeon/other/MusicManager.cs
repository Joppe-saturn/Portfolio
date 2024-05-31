using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private List<AudioClip> levelMusic;

    private int level;
    private int levelCheck;

    private void Start()
    {
        level = LevelBuilder.instance.level;
    }

    private void Update()
    {
        if(level != levelCheck)
        {
            gameObject.GetComponent<AudioSource>().Stop();
            gameObject.GetComponent<AudioSource>().clip = levelMusic[level];
            gameObject.GetComponent<AudioSource>().Play();
            gameObject.GetComponent<AudioSource>().loop = true;
            levelCheck = level;
        }
    }
}
