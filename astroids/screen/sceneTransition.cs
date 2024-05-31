using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneTransition : MonoBehaviour
{
    [SerializeField] private string automaticSceneTransition;
    private void Start()
    {
        if (automaticSceneTransition != null && automaticSceneTransition != "no")
        {
            SceneManager.LoadScene(automaticSceneTransition);
        }
    }

    private void Update()
    {
        
    }

    public void transistionToScene(string scene)
    {
        if(scene == "Quit") 
        { 
            Application.Quit();
        } else
        {
            SceneManager.LoadScene(scene);
        }
    }
}
