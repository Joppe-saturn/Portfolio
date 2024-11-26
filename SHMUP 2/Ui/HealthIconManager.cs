using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthIconManager : MonoBehaviour
{
    [SerializeField] private GameObject icon;
    private Player player;

    private void Start()
    {
        player = FindFirstObjectByType<Player>();
    }

    private void Update()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < player.health; i++)
        {
            GameObject newIcon = Instantiate(icon);
            newIcon.transform.SetParent(transform);
            newIcon.transform.position += new Vector3(75 * i, 0, 0);
        }
    }
}
