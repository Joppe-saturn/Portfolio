using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPlatformGenerator : MonoBehaviour
{
    [SerializeField] private GameObject bolder;
    [SerializeField] private int bolderCount;

    private void Start()
    {
        for(int i = 0; i < bolderCount; i++)
        {
            GameObject bolderInstance = Instantiate(bolder, new Vector3(transform.position.x + Random.Range(-45.0f, 45.0f), transform.position.y + Random.Range(10.0f, 11.0f), transform.position.z + Random.Range(-11.0f, 11.0f)), Quaternion.identity);
            bolderInstance.transform.parent = transform;
        }
    }
}
