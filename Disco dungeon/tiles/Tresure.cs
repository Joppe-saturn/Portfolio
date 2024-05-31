using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tresure : MonoBehaviour
{
    [SerializeField] private GameObject particleWhenCollected;

    public void GetCollected()
    {
        Instantiate(particleWhenCollected, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
