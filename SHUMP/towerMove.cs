using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class towerMove : MonoBehaviour
{
    [SerializeField] private Vector3 speed;
    [SerializeField] private float despawn;

    void Start()
    {
        
    }

    void FixedUpdate()
    {
        transform.position += speed / 50;

        if(transform.position.x < despawn)
        {
            Destroy(gameObject);
        }
    }
}
