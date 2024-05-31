using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ufo : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float time;

    private void Update()
    {
        transform.position += new Vector3(0, 0, -speed);
    }

    private void FixedUpdate()
    {
        time--;
        if (time < 0) 
        { 
            Destroy(gameObject);
        }
    }
}
