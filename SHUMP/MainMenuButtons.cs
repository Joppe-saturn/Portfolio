using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuButtons : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadSceneAsync(1);
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void Lvl_Select()
    {
        SceneManager.LoadSceneAsync(3);
    }
    public void Credits()
    {
SceneManager.LoadSceneAsync(6);
    }
}
