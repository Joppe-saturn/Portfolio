using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaveManager : MonoBehaviour
{
    [System.Serializable]
    public class EnemyInstance
    {
        public GameObject enemy;
        public Vector3 spawnPosition;
        public float extraSpawnTime;
    }

    [System.Serializable]
    public class Wave
    {
        public List<EnemyInstance> enemies;
        public float spawnTime;
    }

    [SerializeField] private List<Wave> waves;
    private List<GameObject> currentEnemies = new List<GameObject>();
    private int enemiesToSpawn = 0;

    public bool playerHasWon = false;

    private void Start()
    {
        StartCoroutine(StartWaveCycle());
    }

    private IEnumerator StartWaveCycle()
    {
        for (int i = 0; i < waves.Count; i++)
        {
            yield return new WaitForSeconds(waves[i].spawnTime);
            currentEnemies.Clear();
            enemiesToSpawn = 0;
            for (int j = 0; j < waves[i].enemies.Count; j++)
            {
                enemiesToSpawn++;
                StartCoroutine(SpawnEnemy(waves[i].enemies[j]));
            }
            while(enemiesToSpawn > 0)
            {
                yield return null;
            }
            bool canSpawnNewWave = false;
            while (!canSpawnNewWave)
            {
                canSpawnNewWave = true;
                for(int j = 0; j < currentEnemies.Count; j++)
                {
                    if (currentEnemies[j] != null)
                    {
                        canSpawnNewWave = false;
                    }
                }
                yield return null;
            }
        }
        playerHasWon = true;
        PlayerPrefs.SetInt(SceneManager.GetActiveScene().name, 2);
        PlayerPrefs.SetInt("Level " + SceneManager.GetActiveScene().buildIndex, 1);

        StartCoroutine(FindFirstObjectByType<SpecialScreenManager>().ShowScreen(1, 2.0f));
        FindFirstObjectByType<Player>().invincibility = true;
    }

    private IEnumerator SpawnEnemy(EnemyInstance enemy)
    {
        yield return new WaitForSeconds(enemy.extraSpawnTime);
        GameObject newEnemy = Instantiate(enemy.enemy, enemy.spawnPosition + new Vector3(10, 0, 0), Quaternion.identity);
        newEnemy.GetComponent<BaseEnemy>().startPos = enemy.spawnPosition;
        currentEnemies.Add(newEnemy);
        enemiesToSpawn--;
    }
}
