using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MoveCamera : MonoBehaviour
{
    [SerializeField] private float speed;

    [SerializeField] private GameObject player;

    private float zoomTimer = 0;
    [SerializeField] private Vector2 maxZoom;

    private void Start()
    {

    }

    private void Update()
    {
        transform.position += new Vector3(transform.forward.x, 0, transform.forward.z) * Input.GetAxisRaw("Vertical") * speed;
        transform.position += transform.right * Input.GetAxisRaw("Horizontal") * speed;
        
        if (Input.GetKey(KeyCode.Mouse1))
        {
            transform.Rotate(0, Input.GetAxis("Mouse X") * 2, 0, Space.World);
        }

        if (Input.mouseScrollDelta.y > 0 && zoomTimer > maxZoom.x)
        {
            transform.position -= (transform.position - player.transform.position).normalized;
            zoomTimer -= 1;
        }
        if (Input.mouseScrollDelta.y < 0 && zoomTimer < maxZoom.y)
        {
            zoomTimer += 1;
            transform.position += (transform.position - player.transform.position).normalized;
        }
    }
}
