using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreenManager : MonoBehaviour
{
    public void BackToTitle()
    {
        SceneManager.LoadScene("LevelSelect");
    }

    public void NextLevel(string level)
    {
        SceneManager.LoadScene(level);
    }
}
