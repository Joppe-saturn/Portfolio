using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatEnemy : BaseEnemy
{
    [SerializeField] private float speed;
    private void Awake()
    {
        transform.position = new Vector3(startPos.x + 10, startPos.y, startPos.z);
        StartCoroutine(StartMoveCycle());

        StartCoroutine(StartShootCycle());
    }

    private IEnumerator StartMoveCycle()
    {
        float i = 0;
        while (Mathf.Abs((transform.position - startPos).magnitude) > 0.1f)
        {
            transform.position = new Vector3(transform.position.x + (startPos.x - transform.position.x) / 500.0f * speed + Mathf.Sin(i * 10.0f) / 30.0f, transform.position.y + (startPos.y - transform.position.y) / 500.0f * speed + Mathf.Sin(i * 10.0f) / 52.6f, transform.position.z + (startPos.z - transform.position.z) / 500.0f * speed);
            yield return null;
        }
        while (!dead)
        {
            i += 0.0001f * speed * Time.deltaTime * 200.0f;
            transform.position = new Vector3(transform.position.x + (startPos.x - transform.position.x) / 50.0f * speed + Mathf.Sin(i * 10.0f) / 30.0f, transform.position.y + (startPos.y - transform.position.y) / 50.0f * speed + Mathf.Sin(i * 10.0f) / 52.6f, transform.position.z + (startPos.z - transform.position.z) / 50.0f * speed);
            yield return null;
        }
    }

    private IEnumerator StartShootCycle()
    {
        GameObject player = FindFirstObjectByType<Player>().gameObject;
        Player player1 = player.GetComponent<Player>();
        while (!player1.dead)
        {
            yield return new WaitForSeconds(coolDown);
            Shoot(player.transform.position);
        }
    }
}
