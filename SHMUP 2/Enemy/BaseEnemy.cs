using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class BaseEnemy : Shooting
{
    [SerializeField] public Vector3 startPos;
    [SerializeField] public int points;
    public bool dead;
    [SerializeField] private GameObject deathParticle;
    public override void Damage(int damage)
    {
        base.Damage(damage);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            FindFirstObjectByType<Player>().Damage(1);
            points = 0;
            Damage(25);
        }
    }

    public override void OnDeath()
    {
        dead = true;
        FindFirstObjectByType<Player>().AddScore(points);
        points = 0;
        StartCoroutine(Die());
    }

    private IEnumerator Die()
    {
        while(transform.localScale.magnitude > 0.1f)
        {
            transform.localScale /= 1.01f;
            yield return null;
        }
        Instantiate(deathParticle, transform.position, quaternion.identity);
        transform.position += new Vector3(1000.0f, 0, 0);
        yield return new WaitForSeconds(2.0f);
        Destroy(gameObject);
    }
}
