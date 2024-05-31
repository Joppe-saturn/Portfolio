using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Material noiseMaterial;
    [SerializeField] private float noiseDegrees;
    private float noise;

    public bool isClosest;

    void Start()
    {
        GetComponent<Animator>().SetBool("isEnemy", true);
    }

    void Update()
    {
        GetComponent<Animator>().SetFloat("speed", GetComponent<NavMeshAgent>().velocity.magnitude);
        if (player.GetComponent<moveToMouse>().die)
        {
            if(noise > 0 && isClosest)
            {
                noise -= noiseDegrees;
                noiseMaterial.SetFloat("_intensity", noise);
            }
        } else
        {
            noise = 5 / (player.transform.position - transform.position).magnitude;
        }

        if (isClosest)
        {
            noiseMaterial.SetFloat("_intensity", noise);
        }
    }

    public virtual void MoveTo(Vector3 pos)
    {
        if(!player.GetComponent<moveToMouse>().die)
        {
            GetComponent<NavMeshAgent>().SetDestination(pos);
        }
    }
}
