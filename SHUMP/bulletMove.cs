using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletMove : MonoBehaviour
{
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float bulletDespawnDistance;

    private void FixedUpdate()
    {
        transform.position -= new Vector3(bulletSpeed / 50, 0, 0);

        if(transform.position.x < bulletDespawnDistance * -1 || transform.position.x > bulletDespawnDistance)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "playerBullet")
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
