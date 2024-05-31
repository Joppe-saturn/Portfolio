using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class door : MonoBehaviour
{
    [SerializeField] private float interactionDistance;
    [SerializeField] private GameObject player;
    private float distanceToPlayer;

    private Animator animator;

    [SerializeField] private string openDirection;

    private enum doorState
    {
        Locked,
        Closed,
        Open
    }

    private doorState state = doorState.Locked;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        distanceToPlayer = (player.transform.position - transform.position).magnitude;
        switch (state)
        {
            case doorState.Locked:
                HandleDoorLocked();
                break;
            case doorState.Closed:
                HandleDoorClosed();
                break;
            case doorState.Open:
                HandleDoorOpen();
                break;
        }
    }

    private void HandleDoorLocked()
    {
        if(openDirection == "z")
        {
            animator.SetBool("open1", false);
            animator.SetBool("open2", false);
        } else
        {
            animator.SetBool("open1", false);
            animator.SetBool("open2", false);
        }
        GetComponent<NavMeshObstacle>().carving = true;
        if (Input.GetKeyDown(KeyCode.Space) && distanceToPlayer < interactionDistance)
        {
            player.GetComponent<moveToMouse>().TryToOpenDoor(gameObject);
        }
    }

    private void HandleDoorClosed()
    {
        if (openDirection == "z")
        {
            animator.SetBool("open1", false);
            animator.SetBool("open2", false);
        }
        else
        {
            animator.SetBool("open1", false);
            animator.SetBool("open2", false);
        }
        GetComponent<NavMeshObstacle>().carving = true;
        if (Input.GetKeyDown(KeyCode.Space) && distanceToPlayer < interactionDistance)
        {
            state = doorState.Open;
        }
    }

    private void HandleDoorOpen()
    {
        //this one checks what way to open the door so that it never opens towards you
        if (openDirection == "z")
        {
            if (transform.position.z < player.transform.position.z)
            {
                animator.SetBool("open1", true);
            }
            else
            {
                animator.SetBool("open2", true);
            }
        }
        else
        {
            if (transform.position.x < player.transform.position.x)
            {
                animator.SetBool("open1", true);
            }
            else
            {
                animator.SetBool("open2", true);
            }
        }

        //this one makes sure you can walk through the door.
        GetComponent<NavMeshObstacle>().carving = false;

        //this one closes the door if you are too far away
        if (distanceToPlayer > interactionDistance)
        {
            state = doorState.Closed;
        }
    }

    public void Unlock()
    {
        state = doorState.Open;
    }
}
