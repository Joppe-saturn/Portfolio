using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingEnemy : Enemy
{
    [SerializeField] private float waitingTime;
    [SerializeField] private float radius;
    private float timer;

    void Start()
    {
        
    }

    void FixedUpdate()
    {
        timer--;
        if(timer < 0)
        {
            timer = waitingTime;
            base.MoveTo(new Vector3(transform.position.x + Random.Range(-radius,radius), 0, transform.position.z + Random.Range(-radius, radius)));
        }
    }
}
