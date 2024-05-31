using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraFollow : MonoBehaviour
{
    [SerializeField] private Transform m_PLayerTransform = null;
    [SerializeField] private float m_Offset = -10f;
    [SerializeField] private float cameraOffset;

    private void Start()
    {
        
    }

    private void Update()
    {
        float x = transform.position.x + (m_PLayerTransform.position.x - transform.position.x) / cameraOffset;
        float y = transform.position.y + (m_PLayerTransform.position.y - transform.position.y) / cameraOffset;
        transform.position = new Vector3(x, y, m_Offset);
    }
}
