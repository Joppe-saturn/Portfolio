using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastBullet : MonoBehaviour
{
    public bool isDone = false;
    public List<GameObject> walls = new List<GameObject>();
    public bool isShrinking = false;

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "wall" && other.GetComponent<MeshRenderer>().isVisible)
        {
            walls.Add(other.gameObject);
        }
        
        isDone = true;
    }

    private void Update()
    {
        if(isShrinking)
        {
            isShrinking = false;
            StartCoroutine(shrink());
        }
    }

    private IEnumerator shrink()
    {
        while(transform.localScale.x > 0.1)
        {
            transform.localScale -= new Vector3(0.02f, 0.02f, 0);
            yield return new WaitForSeconds(0.02f);
        }
        Destroy(gameObject);
    }
}
