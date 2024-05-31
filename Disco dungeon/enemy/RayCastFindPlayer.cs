using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastFindPlayer : MonoBehaviour
{
    public bool hasHitWall = false;

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "wall" && other.GetComponent<MeshRenderer>().isVisible) 
        {
            hasHitWall = true;
        }
    }
}
