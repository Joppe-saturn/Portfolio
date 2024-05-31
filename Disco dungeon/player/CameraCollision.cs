using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollision : MonoBehaviour
{
    [SerializeField] private float tileOffset;

    private float order;
    
    private List<GameObject> gameObjects;

    void Start()
    {
        order = 0;
        StartCoroutine(die());
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag != "camera")
        {
            gameObjects.Add(other.gameObject);
        }
        other.gameObject.GetComponent<TileProperties>().order = order;
        other.gameObject.GetComponent<TileProperties>().hasBeenTouched = true;
        order += tileOffset;
    }

    private IEnumerator die()
    {
        yield return null;
        
        Destroy(gameObject);
    }
}
