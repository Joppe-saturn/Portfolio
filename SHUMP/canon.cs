using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private float buttonCooldown;
    private float buttonCooldownCheck;

    public Transform bulletSpawnPoint;
    public GameObject bulletPrefab;
    public float bulletSpeed = 10;

    private void Start()
    {
        buttonCooldownCheck = buttonCooldown;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space) && buttonCooldownCheck < 0)
        {
            Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
            buttonCooldownCheck = buttonCooldown;
        }
    }

    private void FixedUpdate()
    {
        buttonCooldownCheck --;
    }
}