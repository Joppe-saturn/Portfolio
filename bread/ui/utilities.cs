using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class utilities : MonoBehaviour
{
    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Level 1");
    }

    public void StopGame()
    {
        Application.Quit();
        Debug.Log("stop");
    }

    public void NextLevel()
    {
        SceneManager.LoadScene("menu");
    }

    public void QuitToTitle()
    {
        SceneManager.LoadScene("menu");
    }
}
