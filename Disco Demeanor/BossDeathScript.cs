using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDeathScript : MonoBehaviour
{
    [SerializeField] private GameObject fuel;
    private void Update()
    {
        if(GetComponent<HealthManager>().health <= 0)
        {
            if(UIManager.instance != null) UIManager.instance.ToggleBossHP(false);
            Instantiate(fuel,transform.position, Quaternion.identity);
            Die();
        }
        
    }

    private void Die()
    {
        gameObject.SetActive(false);
    }
}
