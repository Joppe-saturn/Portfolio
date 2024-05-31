using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mariokart : MonoBehaviour
{
    [SerializeField] private int countdown;

    void Start()
    {
        StartCoroutine(countTo(countdown));
    }

    private IEnumerator countTo(int seconds)
    {
        while(0 < seconds)
        {
                Debug.Log(seconds);
                seconds--;
                yield return new WaitForSeconds(1);
        }
        Debug.Log("GO!!!");
    }
}
