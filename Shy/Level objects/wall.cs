using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wall : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private List<Material> materials = new List<Material>();

    private void Start()
    {
        
    }

    private void Update()
    {
        if(player.transform.position.z < transform.position.z)
        {
            GetComponent<MeshRenderer>().material = materials[1];
        } else
        {
            GetComponent<MeshRenderer>().material = materials[0];
        }
    }
}
