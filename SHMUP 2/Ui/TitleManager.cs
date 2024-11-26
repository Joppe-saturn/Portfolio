using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("LevelSelect");
    }

    public void Reset()
    {
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings - 2; i++)
        {
            PlayerPrefs.SetInt("Level " + (i + 1), 0);
        }
        PlayerPrefs.SetInt("Level 1", 1);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
