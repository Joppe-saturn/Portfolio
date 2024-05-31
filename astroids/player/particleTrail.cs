using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particleTrail : MonoBehaviour
{
    private movement player;

    private ParticleSystem myParticle;

    void Start()
    {
        player = transform.parent.GetComponent<movement>();
        myParticle = gameObject.GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if(player.isMoving)
        {
            myParticle.Play();
        } else
        {
            myParticle.Stop();
        }
    }
}
