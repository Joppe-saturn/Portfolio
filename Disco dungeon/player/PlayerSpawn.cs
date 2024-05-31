using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    private void Awake()
    {
        transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
    }
}
