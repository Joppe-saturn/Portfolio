using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectButtons : MonoBehaviour
{
    public void Lvl1()
    {
        SceneManager.LoadSceneAsync(1);
    }
    //public void Boss2()
    //{
    //   SceneManager.LoadSceneAsync();
    //}
    //public void Boss3()
    //{
    //    SceneManager.LoadSceneAsync();
    //}
    //public void Boss4()
    //{
    //    SceneManager.LoadSceneAsync();
    //}
    //public void Boss5()
    //{
    //    SceneManager.LoadSceneAsync();
    //}
    public void Back()
    { SceneManager.LoadSceneAsync(0);
    }
}
