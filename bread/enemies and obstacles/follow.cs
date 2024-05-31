using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class follow : MonoBehaviour
{
    [SerializeField] private Transform m_PLayerTransform = null;
    [SerializeField] private Player player;
    [SerializeField] private float m_Offset = -10f;
    [SerializeField] private float knifeFollowSpeed;

    private void Start()
    {

    }

    private void Update()
    {
        if(player.hasCheckPointBeenReached)
        {
            float x = transform.position.x + ((m_PLayerTransform.position.x - transform.position.x) / knifeFollowSpeed) * Time.deltaTime;
            float y = transform.position.y + ((m_PLayerTransform.position.y - transform.position.y) / knifeFollowSpeed) * Time.deltaTime;
            transform.position = new Vector3(x, y, m_Offset);
            if(player.hasDied)
            {
                transform.position = new Vector3(-12, 62, 0);
            }
        }
    }
}
