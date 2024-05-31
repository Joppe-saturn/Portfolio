using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class energyScript : MonoBehaviour
{
    [SerializeField] private Player player;

    private void Start()
    {

    }

    private void Update()
    {
        transform.localScale = new Vector3(1, player.energy * 2, 1);
    }
}
