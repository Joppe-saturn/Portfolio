using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCollision : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Gets spawned by the PlayerMovement script to check for walls. When adding a wall, make sure it has the "Wall" tag,
        // otherwise the script won't recognize it.
        if(other.tag == "Wall")
        {
            gameObject.SetActive(false);
        }
    }
}
