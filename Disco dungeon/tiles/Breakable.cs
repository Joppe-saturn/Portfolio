using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    [SerializeField] public GameObject replaceMe;

    private void Start()
    {
        transform.localScale = new Vector3 (1, transform.position.y, 1);
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        GetComponent<MeshRenderer>().enabled = false;
    }

    private void Update()
    {
        if(LevelBuilder.instance.hasGameStartedYet)
        {
            GetComponent<MeshRenderer>().enabled = true;
        }
    }
    public void Break()
    {
        Destroy(gameObject);
    }
}
