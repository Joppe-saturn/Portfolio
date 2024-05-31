using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clone : MonoBehaviour
{
    [SerializeField] private GameObject cloner;

    private void Start()
    {
        float counter = 3;
        for(int i = 0; i < 100; i++)
        {
            GameObject knife = Instantiate(cloner, transform.position, transform.rotation);
            knife.transform.position = new Vector3(-5, counter, 0);
            knife.transform.rotation = Quaternion.Euler(0, 0, -220);
            counter += 1;
        }
        counter = 3;
        for (int i = 0; i < 100; i++)
        {
            GameObject knife = Instantiate(cloner, transform.position, transform.rotation);
            knife.transform.position = new Vector3(5, counter, 0);
            knife.transform.rotation = Quaternion.Euler(0, 0, -35);
            counter += 1;
        }
    }

    private void Update()
    {

    }
}
