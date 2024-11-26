using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bolder : MonoBehaviour
{
    private void Start()
    {
        transform.LookAt(new Vector3(Random.Range(-60.0f, 60.0f), Random.Range(-60.0f, 60.0f), Random.Range(-60.0f, 60.0f)));
        transform.localScale *= Random.Range(0.5f, 1.5f);
    }
}
