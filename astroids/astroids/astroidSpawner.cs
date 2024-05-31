using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class astroidSpawner : MonoBehaviour
{
    [SerializeField] private GameObject astroid;
    [SerializeField] private GM gameManager;

    private List<GameObject> spawnedAstroid;

    public float maxAstroids;

    private bool scoreIncrease;

    private boss bossObject;
    private void Start()
    {
        spawnedAstroid = new List<GameObject>();
        bossObject = FindObjectOfType<boss>();
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "Main") 
        {
            if (spawnedAstroid.Count < maxAstroids && !bossObject.isBossActive)
            {
                transform.rotation = Quaternion.EulerRotation(0, Random.Range(-360f, 360f), 0);
                transform.position = transform.forward * -25;
                spawnedAstroid.Add(Instantiate(astroid, transform.position, transform.rotation));
            }
        }
        
        for (int i = 0; i < spawnedAstroid.Count; ++i)
        {
            if (spawnedAstroid[i] == null)
            {
                spawnedAstroid.RemoveAt(i);
            }
        }

        if((gameManager.score / 100) % 10 == 9)
        {
            if(scoreIncrease)
            {
                maxAstroids++;
                scoreIncrease = false;
            }
        } else
        {
            scoreIncrease = true;
        }
    }

    public void changeAmountAStroid(float amount)
    {
        maxAstroids += amount;
    }
}
