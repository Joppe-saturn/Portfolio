using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(FadeWhenLevelEnds());
        StartCoroutine(FadeWhenPlayerEnds());
        StartCoroutine(CheckForBoss());
    }

    private IEnumerator CheckForBoss()
    {
        while(!FindFirstObjectByType<MiniBossManager>())
        {
            yield return null;
        }
        for(int i = 0; i < 100; i++)
        {
            GetComponent<AudioSource>().volume -= 0.01f;
            yield return new WaitForSeconds(0.02f);
        }
        transform.GetChild(0).GetComponent<AudioSource>().Play();
    }

    private IEnumerator FadeWhenLevelEnds()
    {
        WaveManager waveManager = FindFirstObjectByType<WaveManager>();
        while (!waveManager.playerHasWon)
        {
            yield return null;
        }
        for (int i = 0; i < 100; i++)
        {
            GetComponent<AudioSource>().volume -= 0.01f;
            transform.GetChild(0).GetComponent<AudioSource>().volume -= 0.01f;
            yield return new WaitForSeconds(0.02f);
        }
        transform.GetChild(1).GetComponent<AudioSource>().Play();
    }
    private IEnumerator FadeWhenPlayerEnds()
    {
        Player player = FindFirstObjectByType<Player>();
        while (!player.dead)
        {
            yield return null;
        }
        for (int i = 0; i < 100; i++)
        {
            GetComponent<AudioSource>().volume -= 0.01f;
            transform.GetChild(0).GetComponent<AudioSource>().volume -= 0.01f;
            yield return new WaitForSeconds(0.02f);
        }
        transform.GetChild(2).GetComponent<AudioSource>().Play();
    }
}
