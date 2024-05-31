using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileProperties : MonoBehaviour
{
    [SerializeField] private bool changeColor;
    [SerializeField] private List<Material> materials = new List<Material>();
    public float bpm;

    private float timer;
    public int materialCounter;

    public bool hasBeenTouched;
    public float fallHeight;
    public float timeToFall;
    public float order;

    private void Start()
    {
        if(materials.Count > 0 )
        {
            gameObject.GetComponent<MeshRenderer>().material = materials[materialCounter];
        }
    }
    private void FixedUpdate()
    {
        if(hasBeenTouched)
        {
            hasBeenTouched = false;
            StartCoroutine(fall());
        }

        timer++;
        if (timer > 50/(bpm/60) && changeColor)
        {
            materialCounter++;
            if(materialCounter >= materials.Count)
            {
                materialCounter = 0;
            }
            gameObject.GetComponent<MeshRenderer>().material = materials[materialCounter];
            timer = 0;
        }
    }



    private IEnumerator fall()
    {
        transform.position = new Vector3 (transform.position.x, fallHeight, transform.position.z);
        yield return new WaitForSeconds(order);
        while(transform.position.y > 0)
        {
            transform.position -= new Vector3(0, 0.1f, 0);
            if(transform.position.y < 0)
            {
                transform.position = new Vector3(transform.position.x, 0, transform.position.z);
            }
            yield return new WaitForSeconds(timeToFall / (fallHeight*10));
        }
    }
}
