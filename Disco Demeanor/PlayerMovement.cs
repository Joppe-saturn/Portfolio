using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float speed = 1;
    [SerializeField] private float baseSpeed = 10f;

    [SerializeField] private GameObject ray;
    [SerializeField] private int framesBeforeMaxSpeed = 20;

    [SerializeField] private float rayDistance = 0.1f;
    private bool buttonPressed;

    private void Update()
    {
        //this one checks what buttons you pressed to call the move function
        buttonPressed = false;
        if (Input.GetKey(KeyCode.W))
        {
            StartCoroutine(Move(new Vector3(0, 0, rayDistance)));
            buttonPressed = true;
        }
        if (Input.GetKey(KeyCode.S))
        {
            StartCoroutine(Move(new Vector3(0, 0, -rayDistance)));
            buttonPressed = true;
        }
        if (Input.GetKey(KeyCode.D))
        {
            StartCoroutine(Move(new Vector3(rayDistance, 0, 0)));
            buttonPressed = true;
        }
        if (Input.GetKey(KeyCode.A))
        {
            StartCoroutine(Move(new Vector3(-rayDistance, 0, 0)));
            buttonPressed = true;
        }
        speed += ((100 * baseSpeed) - speed) / framesBeforeMaxSpeed;

        /*if (Input.GetKey(KeyCode.W))
        {
            if(GetComponent<Rigidbody>().velocity.z < maxSpeed)
            {
                GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, baseSpeed));
            }
            buttonPressed = true;
        }
        if (Input.GetKey(KeyCode.S))
        {
            if (GetComponent<Rigidbody>().velocity.z > -maxSpeed)
            {
                GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, -baseSpeed));
            }
            buttonPressed = true;
        }
        if (Input.GetKey(KeyCode.D))
        {
            if (GetComponent<Rigidbody>().velocity.x < maxSpeed)
            {
                GetComponent<Rigidbody>().AddForce(new Vector3(baseSpeed, 0, 0));
            }
            buttonPressed = true;
        }
        if (Input.GetKey(KeyCode.A))
        {
            if (GetComponent<Rigidbody>().velocity.x > -maxSpeed)
            {
                GetComponent<Rigidbody>().AddForce(new Vector3(-baseSpeed, 0, 0));
            }
            buttonPressed = true;
        }*/

        GetComponent<Rigidbody>().velocity = Vector3.zero;

        if (!buttonPressed)
        {
            speed = 1;
        }
    }

    public IEnumerator Move(Vector3 direction)
    {
        //this one instantiates the ray
        GameObject tempRay = Instantiate(ray, transform.position + direction * speed * Time.deltaTime, Quaternion.identity);
        tempRay.transform.localScale = transform.localScale / 2;
        yield return null;
        //this one checks if the ray hasn't hit anything
        if(tempRay.activeSelf)
        {
            transform.position += direction * speed * Time.deltaTime;
        }
        //this one kills the ray (very bloody oh no)
        Destroy(tempRay);
    }
}
