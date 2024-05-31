using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class background : MonoBehaviour
{
    [SerializeField] private List<GameObject> towers;
    [SerializeField] private int towerFrequency;
    [SerializeField] private Vector3 towerRange;
    [SerializeField] private Vector3 towerStartPos;
    [SerializeField] private float amountOfStartBuildings;
    [SerializeField] private Vector3 startBuildingsPos;
    [SerializeField] private Vector3 sizeRange;

    void Start()
    {
        //this one loads a bunch of buildings before the game starts
        for (int i = 0; i < amountOfStartBuildings; i++)
        {
            GameObject building = Instantiate(towers[Random.Range(0, towers.Count)], new Vector3(Random.Range(startBuildingsPos.x * -1, startBuildingsPos.x), Random.Range(startBuildingsPos.y * -1, startBuildingsPos.y) + towerStartPos.y, Random.Range(startBuildingsPos.z * -1, startBuildingsPos.z) + towerStartPos.z), Quaternion.Euler(-90, 0, 0));
            building.transform.localScale += new Vector3(Random.Range(0, sizeRange.x), Random.Range(0, sizeRange.y), Random.Range(0, sizeRange.z));
            if(building.transform.position.z < 15)
            {
                building.transform.position += new Vector3(0, 0, 10);
            }
        }
    }

    private void FixedUpdate()
    {
        //this one loads a bunch of buildings after the game starts
        if(Random.Range(0, towerFrequency) == 1)
        {
            GameObject building = Instantiate(towers[Random.Range(0, towers.Count)], new Vector3(Random.Range(towerRange.x * -1, towerRange.x) + towerStartPos.x, Random.Range(towerRange.y * -1, towerRange.y) + towerStartPos.y, Random.Range(towerRange.z * -1, towerRange.z) + towerStartPos.z), Quaternion.Euler(-90, 0, 0));
            building.transform.localScale += new Vector3(Random.Range(0, sizeRange.x), Random.Range(0, sizeRange.y), Random.Range(0, sizeRange.z));
            if (building.transform.position.z < 25)
            {
                building.transform.position += new Vector3(0, 0, 10);
            }
        }
    }
}
