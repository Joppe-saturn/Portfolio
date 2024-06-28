using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Bulletmove : MonoBehaviour
{
    public float speed;

    public float timeBeforeDelete = 5f;
    public float deletionTimeAfterCollision = 0f;
    [SerializeField] private float deleteTimeImpact = 1f;
    public bool isFromPlayer;
    public int damage;
    public int pierceAmount;
    public float timer = 0;
    public bool deleteVisualOnImpact = true;
    public bool deleteVisualOnTimeUp = true;
    public bool collidesWithWalls = true;

    [SerializeField] GameObject impactEffect;

    [Header ("Visual Effect Graph and Event name")] 
    [SerializeField] VisualEffect visuals;
    [SerializeField] string stopVisualEvent;

    [Header("Collider to be disabled on impact")]
    [SerializeField] Collider hitbox;

    [SerializeField] private GameObject audioManager;
    [SerializeField] private List<AudioClip> audioClips = new List<AudioClip>();
    //audio 0 is for hitting a wall

    private void Start()
    {
        //Physics solution to movement for better collision, make sure to set RB's drag to 0.
        Rigidbody rb = GetComponent<Rigidbody>();
        if(rb != null) rb.velocity = transform.forward * speed;
    }
    private void Update()
    {
        timer += Time.deltaTime;
        if(timeBeforeDelete < timer)
        {
            Dissapear(false);
        }
    }


    private void Dissapear(bool hitSomething)
    {
        if (visuals != null)
        { 
            if(hitSomething && deleteVisualOnImpact)
            {
                visuals.Reinit();
                visuals.SendEvent(stopVisualEvent);
            }
            if (!hitSomething && deleteVisualOnTimeUp)
            {
                visuals.Reinit();
                visuals.SendEvent(stopVisualEvent);
            }
        }
        if(hitbox != null)
        {
            hitbox.enabled = false;
        }
        if (!hitSomething || (hitSomething && deleteVisualOnImpact))
        {
            Destroy(gameObject, deletionTimeAfterCollision);
        }
    }

    //Uses the collision point and the normal vector of the impacted object to spawn the impact effect
    private void ImpactVisual(Vector3 pos, Vector3 dir)
    {
        Quaternion qDir = Quaternion.FromToRotation(Vector3.up, dir);
        if(impactEffect != null) 
        {
        GameObject flash = Instantiate(impactEffect, pos, qDir);
        Destroy(flash, 2f);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        //this one checks for if the bullet collides with something
        if (other.gameObject.CompareTag("Wall") && collidesWithWalls)
        {
            if (audioManager != null)
            {
                GameObject audioInstance = Instantiate(audioManager);
                audioInstance.GetComponent<AudioSource>().clip = audioClips[0];
                audioInstance.GetComponent<AudioManager>().valuesSet = true;
            }
            Dissapear(true);
            ImpactVisual(other.GetContact(0).point, other.GetContact(0).normal);
        }

        if ((other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Boss")) && isFromPlayer)
        {
            DealDamage(other.gameObject);
            ImpactVisual(other.GetContact(0).point, other.GetContact(0).normal);
        }

        if (other.gameObject.CompareTag("Player") && !isFromPlayer)
        {
            DealDamage(other.gameObject);
            ImpactVisual(other.GetContact(0).point, other.GetContact(0).normal);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Wall") && collidesWithWalls)
        {
            if (audioManager != null)
            {
                GameObject audioInstance = Instantiate(audioManager);
                audioInstance.GetComponent<AudioSource>().clip = audioClips[0];
                audioInstance.GetComponent<AudioManager>().valuesSet = true;
            }
            Dissapear(true);            
        }

        if ((other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Boss")) && isFromPlayer)
        {
            DealDamage(other.gameObject);
        }

        if (other.gameObject.CompareTag("Player") && !isFromPlayer)
        {
            DealDamage(other.gameObject);
        }
    }
    private void DealDamage(GameObject target)
    {
        //Gets the HealthManager script from the target, then lowers the target's health equal to the amount of damage that's being dealt.
        target.GetComponent<HealthManager>().TakeDamage(damage);
        if(pierceAmount == 0)
        {
            Dissapear(true);
        } else
        {
            pierceAmount--;
        }
    }
}
