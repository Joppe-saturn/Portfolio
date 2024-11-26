using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : Health 
{
    [SerializeField] public List<string> Targets;
    [SerializeField] public float coolDown;
    [SerializeField] public int BulletDamage;
    [SerializeField] private GameObject bullet;

    public void Shoot(Vector3 target)
    {
        GameObject bulletInstance = Instantiate(bullet, transform.position, Quaternion.identity);
        bulletInstance.transform.up = target - transform.position;
        bulletInstance.GetComponent<BulletMove>().Targets = Targets;
        bulletInstance.GetComponent<BulletMove>().damage = BulletDamage;
    }
}
