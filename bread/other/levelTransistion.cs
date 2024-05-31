using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.EventSystems.EventTrigger;

public class levelTransistion : MonoBehaviour
{
    [SerializeField] private Player player;
    public GameObject light;
    private float timer;
    [SerializeField] public float transistionToLevel;
    public float currentLevel;

    private void Start()
    {
        timer = 0;
    }

    private void Update()
    {
        currentLevel = transistionToLevel - 1;
        if (player.isPaused)
        {
            return;
        }
        if(timer < 0)
        {
            Instantiate(light, transform.position, transform.rotation);
            timer = 1;
        } else
        {
            timer -= Time.deltaTime;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        string currentScene = "Level " + (transistionToLevel);
        SceneManager.LoadScene(currentScene);
        player.NextLevel();
    }
}
