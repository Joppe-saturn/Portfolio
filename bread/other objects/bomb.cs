using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class bomb : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private Rigidbody2D bomb2;
    [SerializeField] private float bombSpeed;
    [SerializeField] private GameObject explosion;
    [SerializeField] private GameObject[] canDestroy;
    [SerializeField] private GameObject dontDieOnCollisiom;

    private bool hasBeenTouched = false;

    private void Start()
    {

    }

    private void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //this one checks if a bomb makes contact with a player
        if (collision.transform == player.transform)
        {
            if(!hasBeenTouched)
            {
                //this one gives the bomb velocity based on the rotation of the player
                bomb2.transform.rotation = player.transform.rotation * quaternion.Euler(0, 0, 90);

                Vector3 direction = transform.position - player.transform.position;
                direction.Normalize();
                bomb2.AddForce(direction * bombSpeed);
                hasBeenTouched = true;
            }
        } else if(collision.gameObject.tag != dontDieOnCollisiom.gameObject.tag)
        {
            //this one checks if he can destroy the object it collided with
            bool hasNotDestroyed = true;
            for(int i = 0; i < canDestroy.Length;  i++)
            {
                if(hasNotDestroyed)
                {
                    if (collision.transform == canDestroy[i].transform)
                    {
                        canDestroy[i].SetActive(false);
                        Instantiate(explosion, transform.position, transform.rotation);
                        hasNotDestroyed = false;
                    }
                }
            }
            //this one spawns a explosion after hitting something
            Instantiate(explosion, transform.position, transform.rotation);
            bomb2.gameObject.SetActive(false);
        }
    }
}
