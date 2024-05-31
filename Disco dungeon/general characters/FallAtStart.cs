using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallAtStart : MonoBehaviour
{
    private bool isFallen = false;

    private void Start()
    {
        transform.position = new Vector3(transform.position.x, 10f, transform.position.z);
    }

    void Update()
    {
        if (LevelBuilder.instance.canFall && !isFallen)
        {
            StartCoroutine(falling());
            isFallen = true;
        }
    }

    private IEnumerator falling()
    {
        while (transform.position.y > 0.5f)
        {
            GetComponent<BoxCollider>().isTrigger = false;
            GetComponent<Rigidbody>().useGravity = true;
            yield return null;
        }
        GetComponent<BoxCollider>().isTrigger = true;
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
        LevelBuilder.instance.canFall = false;
    }
}
