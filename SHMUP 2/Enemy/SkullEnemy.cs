using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullEnemy : BaseEnemy
{
    [SerializeField] private float speed;
    [SerializeField] private float range;

    private void Start()
    {
        StartCoroutine(Move());
        if(BulletDamage > 0)
        {
            StartCoroutine(ShootCycle());
        }
    }

    private IEnumerator Move()
    {
        float i = 0;
        transform.position += new Vector3(10, 0, 0);
        while(Mathf.Abs((transform.position - startPos).magnitude) > 0.1f)
        {
            transform.position += (startPos - transform.position) / 100.0f * Time.deltaTime * 200.0f;
            yield return null;
        }
        Player player = FindFirstObjectByType<Player>();
        while (!player.dead && !dead)
        {
            i += 0.005f * speed * Time.deltaTime * 200.0f;
            transform.position = new Vector3(transform.position.x - speed / 500.0f * Time.deltaTime * 250.0f, startPos.y + Mathf.Sin(i) * range, 0);
            yield return null;
            if(transform.position.x < -12.5f)
            {
                FindFirstObjectByType<LightHouse>().health--;
                points = 0;
                Damage(100);
                break;
            }
        }
    }

    private IEnumerator ShootCycle()
    {
        while (true)
        {
            Shoot(transform.position);
            yield return new WaitForSeconds(coolDown);
        }
    }
}
