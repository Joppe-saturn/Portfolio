using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    [SerializeField] private GameObject boss;
    [SerializeField] private float sizeMultiplier;
    private HealthManager health;
    private float maxHealth = -1;
    private float actualHealth;

    private void Start()
    {
    }

    private void Update()
    {
        if (boss == null)
        {
            boss = GameObject.FindGameObjectWithTag("Boss");
            transform.localScale = new Vector3(0, transform.localScale.y, 0);
        }
        else
        {
            if (health == null) health = boss.GetComponent<HealthManager>();
            if (maxHealth < 0) maxHealth = health.health;
            actualHealth = health.health;
            transform.localScale = new Vector3(actualHealth * (10 / maxHealth) * sizeMultiplier, transform.localScale.y, 0);
            if (health.health < 0)
            {
                transform.localScale = new Vector3(0, transform.localScale.y, 0);
            }
        }
    }
}
