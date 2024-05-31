using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonScript : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void Quit()
    {
        SceneManager.LoadScene("Main");
    }

    public void begin()
    {
        SceneManager.LoadScene("real level 0");
    }

    public void ExtraQuit()
    {
        Application.Quit();
    }

    public void Retry()
    {
        SceneManager.LoadScene(gameObject.scene.name);
    }
}
