using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniBossManager : BaseEnemy
{
    [SerializeField] private float speed;

    [System.Serializable]
    private enum MiniBoss
    {
        FishBone,
        WhaleBone
    }

    [SerializeField] private MiniBoss m_Boss;
    private GameObject player;

    private void Start()
    {
        player = FindFirstObjectByType<Player>().gameObject;
        switch (m_Boss)
        {
            case MiniBoss.FishBone:
                StartCoroutine(FishBone());
                break;
            case MiniBoss.WhaleBone:
                StartCoroutine(WhaleBone());
                break;
        }
    }

    private IEnumerator FishBone()
    {
        StartCoroutine(FishBoneShoot());
        yield return null;
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
    private IEnumerator FishBoneShoot()
    {
        while (true)
        {
            yield return new WaitForSeconds(coolDown);
            for (int i = 0; i < 3; i++)
            {
                Shoot(player.transform.position);
                yield return new WaitForSeconds(0.5f);
            }
            yield return new WaitForSeconds(1.0f);
            for (int i = 0; i < 3; i++)
            {
                Shoot(player.transform.position);
                yield return new WaitForSeconds(0.5f);
            }
        }
    }

    private IEnumerator WhaleBone()
    {
        float i = 0;
        StartCoroutine(WhaleBoneShoot());
        while (Mathf.Abs((transform.position - startPos).magnitude) > 0.1f)
        {
            transform.position = new Vector3(transform.position.x + (startPos.x - transform.position.x) / 500.0f * speed + Mathf.Sin(i * 10.0f) / 30.0f, transform.position.y + (startPos.y - transform.position.y) / 500.0f * speed + Mathf.Sin(i * 10.0f) / 52.6f, transform.position.z + (startPos.z - transform.position.z) / 500.0f * speed);
            yield return null;
        }
        while (!dead)
        {
            i += 0.0001f * speed * Time.deltaTime * 200.0f;
            transform.position = new Vector3(transform.position.x + (startPos.x - transform.position.x) / 50.0f * speed + Mathf.Sin(i * 10.0f) / 20.0f, transform.position.y + (startPos.y - transform.position.y) / 50.0f * speed + Mathf.Sin(i * 10.0f) / 42.6f, transform.position.z + (startPos.z - transform.position.z) / 40.0f * speed);
            yield return null;
        }
        yield break;
    }
    private IEnumerator WhaleBoneShoot()
    {
        while (true)
        {
            yield return new WaitForSeconds(coolDown);
            for (int j = 0; j < 2; j++)
            {
                for (int i = 0; i < 10; i++)
                {
                    Shoot(player.transform.position);
                    yield return new WaitForSeconds(0.1f);
                }
                yield return new WaitForSeconds(0.5f);
                for (int i = 0; i < 10; i++)
                {
                    Shoot(player.transform.position);
                    yield return new WaitForSeconds(0.1f);
                }
                yield return new WaitForSeconds(coolDown);
            }
            for (int i = 0; i < 10; i++)
            {
                Shoot(new Vector3(player.transform.position.x, -5.0f + 0.25f * i, player.transform.position.z));
                yield return new WaitForSeconds(0.1f);
            }
            yield return new WaitForSeconds(0.5f);
            for (int i = 0; i < 10; i++)
            {
                Shoot(new Vector3(player.transform.position.x, 5.0f - 0.25f * i, player.transform.position.z));
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}
