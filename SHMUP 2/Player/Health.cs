using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour 
{
    [SerializeField] public int health;
    
    public virtual void Damage(int damage)
    {
        health -= damage;
        if (health < 1)
        {
            OnDeath();
        }
    }

    public virtual void OnDeath()
    {

    }
}
