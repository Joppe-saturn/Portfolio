using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float MoveSpeed = 10f;

    [SerializeField] private float lives;
    [SerializeField] private float invinsibility;
    private float invinsibilityTimer;
    private float invinsibilitySycle;
    private bool visibility = true;

    [SerializeField] private Vector2 maxMovement;

    [SerializeField] private GameObject levensManager;
    private string liveText;


    public Rigidbody Rigidbody;

    private void Start()
    {
        levensManager = levensManager.transform.GetChild(1).gameObject;
    }

    void Update()
    {
        /*Vector3 pos = transform.position;*/

        /*if (Input.GetKey(KeyCode.W) && transform.position.y < maxMovement.y)
        {
            pos.y += MoveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S) && transform.position.y > maxMovement.y * -1)
        {
            pos.y -= MoveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D) && transform.position.x < maxMovement.x)
        {
            pos.x += MoveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A) && transform.position.x > maxMovement.x * -1)
        {
            pos.x -= MoveSpeed * Time.deltaTime;
        }
        transform.position = pos;*/

        Rigidbody rg = GetComponent<Rigidbody>();

        if (Input.GetKey(KeyCode.W) && transform.position.y < maxMovement.y)
        {
            rg.velocity = new Vector3(rg.velocity.x, MoveSpeed, 0);
        }
        if (Input.GetKey(KeyCode.S) && transform.position.y > maxMovement.y * -1)
        {
            rg.velocity = new Vector3(rg.velocity.x, -MoveSpeed, 0);
        }
        if (Input.GetKey(KeyCode.D) && transform.position.x < maxMovement.x)
        {
            rg.velocity = new Vector3(MoveSpeed, rg.velocity.y, 0);
        }
        if (Input.GetKey(KeyCode.A) && transform.position.x > maxMovement.x * -1)
        {
            rg.velocity = new Vector3(-MoveSpeed, rg.velocity.y, 0);
        }

        if (lives < 1)
        {
            SceneManager.LoadScene(4);
        }

        liveText = "";
        for(int i = 0; i < lives; i++)
        {
            liveText = liveText + "I";
        }
        levensManager.GetComponent<TextMeshProUGUI>().text = "lives: " + liveText;

        if(visibility)
        {
            GetComponent<MeshRenderer>().enabled = true;
            transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
        } else
        {
            GetComponent<MeshRenderer>().enabled = false;
            transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
        }
    }

    private void FixedUpdate()
    {
        invinsibilityTimer--;
        invinsibilitySycle++;
        if(invinsibilityTimer > 0 && invinsibilitySycle > 25)
        {
            invinsibilitySycle = 0;

            if(visibility)
            {
                visibility = false;
            } else
            {
                visibility = true;
            }
        } else if (invinsibilityTimer < 0)
        {
            visibility = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "enemieBullet" && invinsibilityTimer < 0)
        {
            lives--;
            Destroy(other.gameObject);
            invinsibilityTimer = invinsibility;
        }

        if (other.tag == "enemie" && invinsibilityTimer < 0)
        {
            lives--;
            invinsibilityTimer = invinsibility;
        }
    }
}