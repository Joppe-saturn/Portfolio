using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMove : MonoBehaviour
{
    [SerializeField] private float speed;
    public List<string> Targets = new List<string>();
    public int damage;

    private bool dead = false;

    private void Update()
    {
        transform.position += transform.up * Time.deltaTime * speed;

        if(Mathf.Abs(transform.position.x) > 50.0f)
        {
            transform.localScale /= 1.01f;
        }
        if(dead)
        {
            transform.localScale /= 1.025f;
        }
        if(transform.localScale.x < 0.15f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        for(int i = 0; i < Targets.Count; i++)
        {
            if (other.tag.ToString() == Targets[i] && !dead)
            {
                dead = true;
                if (other.GetComponent<Health>())
                {
                    dead = true;
                    other.GetComponent<Health>().Damage(damage);
                    break;
                }
            }
        }
    }
}
