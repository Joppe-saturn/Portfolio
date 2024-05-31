using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioManager : MonoBehaviour
{
    [SerializeField] private AudioSource gameOverSound;

    private movement player;

    private bool deathSoundActive = false;

    private void Start()
    {
        player = FindObjectOfType<movement>();
    }

    private void Update()
    {
        if(player.lives < 1 && !deathSoundActive)
        {
            gameOverSound.Play();
            deathSoundActive = true;
        }
    }
}
