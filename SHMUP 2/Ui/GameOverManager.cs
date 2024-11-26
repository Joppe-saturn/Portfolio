using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public void RestartLevel()
    {
        Time.timeScale = 1.0f;
        StartCoroutine(FadeOutScreen(2.0f));
    }

    private IEnumerator FadeOutScreen(float time)
    {
        transform.parent.transform.GetChild(2).gameObject.SetActive(true);
        StartCoroutine(transform.parent.transform.GetChild(2).GetComponent<FadeInScreen>().FadeIn(time));
        FindAnyObjectByType<EventSystem>().gameObject.SetActive(false);
        yield return new WaitForSeconds(time);
        string currentScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentScene);
    }
}
