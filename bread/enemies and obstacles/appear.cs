using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class breakwall : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private GameObject trigger2;
    [SerializeField] private GameObject cheese;

    private void Start()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, -10000);
        cheese.SetActive(false);
    }

    private void Update()
    {
        if (player.triggerSquareActive)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
            trigger2.SetActive(false);
            cheese.SetActive(true);
        }
    }
}
