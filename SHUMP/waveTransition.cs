using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waveTransition : MonoBehaviour
{
    [SerializeField] private GameObject wave;
    [SerializeField] private bool testing;
    void Start()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if(wave.transform.childCount < 1 || testing)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }
        }
    }
}
