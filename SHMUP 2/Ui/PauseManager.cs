using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public void Unpause()
    {
        FindFirstObjectByType<Player>().UnPauseOutput();
    }

    public void RestartLevel()
    {
        transform.parent.transform.GetChild(2).gameObject.SetActive(true);
        Time.timeScale = 1.0f;
        string currentScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentScene);
    }

    public void BackToTitle()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("LevelSelect");
    }
}
