using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class HealthManager : MonoBehaviour
{
    public int health;
    public int defaultHealth; //this one should not be edited. It's here to reset the health vairable to it's default value. 
                                //The reason why I didn't do this with the normal health vairable is becuase we already used the health vairable for a lot of stuff and I didn't want to rewrite that-
                                //and cause confusion.
    public int maxHealth;
    [SerializeField] private bool hasInvincibility = false;
    [SerializeField] private float invincibilityTime = 2;
    private bool hasBeenHit = false;
    private float timer;
    private float flashTimer;
    [SerializeField] private float timeBetweenFlash;
    [SerializeField] private bool hitIndicator;
    [SerializeField] private Material material;
    [SerializeField] private float hitTime = 0.2f;
    [SerializeField] private float hitEmmision = 100;
    [SerializeField] private float defaultEmmision = 3;
    [SerializeField] private bool shootOnDeath = false;
    [SerializeField] private VisualEffect hitEffect;
    [SerializeField] private GameObject deathEffect;

    [SerializeField] private GameObject audioManager;
    [SerializeField] private List<AudioClip> audioClips = new List<AudioClip>();
    //audio 0 is for getting hit
    //audio 1 is for dying

    // Toggle UI on Enable
    private void OnEnable()
    {
        if (gameObject.CompareTag("Boss"))
        {
            if (UIManager.instance != null) UIManager.instance.ToggleBossHP(true);
        }
    }
    private void Start()
    {
        defaultHealth = health;
        maxHealth = health;
        if (material != null )
        {
            material.SetFloat("_Emission", defaultEmmision);
        }
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.B) && gameObject.CompareTag("Player"))
        {
            TakeDamage(1);
        }
    }

    public void TakeDamage(int damage)
    {
        if (audioManager != null)
        {
            GameObject audioInstance = Instantiate(audioManager);
            audioInstance.GetComponent<AudioSource>().clip = audioClips[0];
            audioInstance.GetComponent<AudioManager>().valuesSet = true;
        }

        if (gameObject.CompareTag("Enemy"))
        {
            if(GetComponent<EnemyBehavior>().enemyState == EnemyBehavior.CurrentEnemyState.Patrolling)
            {
                GetComponent<EnemyBehavior>().enemyState = EnemyBehavior.CurrentEnemyState.Fighting;
            }
        }

        if(!hasBeenHit)
        {
            if (hitEffect != null)
            {
                hitEffect.SendEvent("OnHit");
            }
            health -= damage;
            if (CompareTag("Player"))
            {
                if(UIManager.instance != null) UIManager.instance.UIPlayerHealthUpdate((int)-damage);
                CameraShake.instance.StartCoroutine(CameraShake.instance.StartShaking(0f, 0.2f));
                FindObjectOfType<PlayerUi>().GetComponent<PlayerUi>().UpdateHealth();
            }
            if (CompareTag("Boss"))
            {
                if(UIManager.instance != null) UIManager.instance.UIBossHealthUpdate((int)-damage);
            }
            if (hasInvincibility)
            {
                hasBeenHit = true;
                StartCoroutine(Invincibility());
            }
        }
        if (health <= 0) Die();

        if(hitIndicator)
        {
            Debug.Log("ouch");
            if(material != null)
            {
                material.SetFloat("_Emission", hitEmmision);
                StartCoroutine(UnEmmision((hitEmmision - defaultEmmision)/hitTime));
            }
        }
    }

    public void HealDamage(int healAmount)
    {
        health = Mathf.Clamp(health + healAmount, 1, maxHealth);
        if (CompareTag("Player"))
        {
            FindObjectOfType<PlayerUi>().GetComponent<PlayerUi>().UpdateHealth();
        }
    }

    private void Die()
    {
        if (audioManager != null)
        {
            GameObject audioInstance = Instantiate(audioManager);
            audioInstance.GetComponent<AudioSource>().clip = audioClips[1];
            audioInstance.GetComponent<AudioSource>().volume = 0.5f;
            audioInstance.GetComponent<AudioManager>().valuesSet = true;
        }
        if (shootOnDeath)
        {
            if (CompareTag("Enemy")) GetComponent<EnemyShooting>().Shoot();
            if (CompareTag("Boss")) GetComponent<BossAttacks>().Shoot();
        }
        if (!gameObject.CompareTag("Boss") && !gameObject.CompareTag("Player"))
        {
            if (GetComponent<DropPickup>() != null) GetComponent<DropPickup>().PickupSpawn();
            if(deathEffect != null) Instantiate(deathEffect, transform.position, Quaternion.identity);
            gameObject.SetActive(false);
        }
    }

    private IEnumerator Invincibility()
    {
        while(timer < invincibilityTime)
        {
            timer += Time.deltaTime;
            flashTimer += Time.deltaTime;

            if (flashTimer > timeBetweenFlash)
            {
                flashTimer = 0;
                for (int i = 0; i < transform.GetChild(0).childCount; i++) 
                {
                    if(transform.GetChild(0).GetChild(i).GetComponent<MeshRenderer>() != null)
                    {
                        MeshRenderer mesh = transform.GetChild(0).GetChild(i).GetComponent<MeshRenderer>();
                        if (mesh.enabled)
                        {
                            mesh.enabled = false;
                        }
                        else
                        {
                            mesh.enabled = true;
                        }
                    }
                }
                
            }

            yield return null;
        }

        timer = 0;
        flashTimer = 0;
        hasBeenHit = false;
        for (int i = 0; i < transform.GetChild(0).childCount; i++)
        {
            if (transform.GetChild(0).GetChild(i).GetComponent<MeshRenderer>() != null)
            {
                transform.GetChild(0).GetChild(i).GetComponent<MeshRenderer>().enabled = true;
            }
        }
    }

    public void RemoveInvinsibility()
    {
        timer += 100000f;
    }

    private IEnumerator UnEmmision(float time)
    {
        while(material.GetFloat("_Emission") > defaultEmmision)
        {
            material.SetFloat("_Emission", material.GetFloat("_Emission") - time * Time.deltaTime);
            yield return null;
        }
    }
}
