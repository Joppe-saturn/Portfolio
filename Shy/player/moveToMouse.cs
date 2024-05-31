using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class moveToMouse : MonoBehaviour
{
    private NavMeshAgent agent;
    [SerializeField] private Camera cam;

    private Animator animator;

    public bool die;

    private List<GameObject> keys = new List<GameObject>();
    [SerializeField] private float keyHeight;
    [SerializeField] private float keySize;
    [SerializeField] private float keySpeed;
    private float keyHeight2;

    private GameObject closestEnemie;

    [SerializeField] private float distanceBeforeDeath;

    [SerializeField] private GameObject boem;

    [SerializeField] private GameObject deadScreen;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            bool checkIfFloor = false;
            List<GameObject> hideObjects = new List<GameObject>();
            while(!checkIfFloor)
            {
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity) && hit.transform.gameObject.tag == "plane")
                {
                    agent.SetDestination(hit.point);
                    checkIfFloor = true;
                } else if(hit.transform.GetComponent<BoxCollider>()  != null)
                {
                    hideObjects.Add(hit.transform.gameObject);
                    hit.transform.GetComponent<BoxCollider>().enabled = false;
                }
            }
            for (int i = 0; i < hideObjects.Count; i++)
            {
                hideObjects[i].GetComponent<BoxCollider>().enabled = true;
            }
        }

        animator.SetFloat("speed", agent.velocity.magnitude);

        if(die)
        {
            agent.SetDestination(transform.position);
        }

        if(!die)
        {
            keyHeight2 = keyHeight;
            for (int i = 0; i < keys.Count; i++)
            {
                keys[i].transform.position = new Vector3(transform.position.x, transform.position.y + keyHeight2, transform.position.z);
                keys[i].transform.rotation *= Quaternion.EulerAngles(0, keySpeed, 0);
                keyHeight2 += keySize;
            }
        }

        for (int i = 0; i < FindObjectsOfType<Enemy>().Length; i++)
        {
            FindObjectsOfType<Enemy>()[i].GetComponent<Enemy>().isClosest = false;
        }
        for (int i = 0; i < FindObjectsOfType<Enemy>().Length; i++)
        {
            if(i != 0)
            {
                if ((FindObjectsOfType<Enemy>()[i].transform.position - transform.position).magnitude < (closestEnemie.transform.position - transform.position).magnitude)
                {
                    closestEnemie = FindObjectsOfType<Enemy>()[i].gameObject;
                }
            } else
            {
                closestEnemie = FindObjectsOfType<Enemy>()[i].gameObject;
            }
        }
        closestEnemie.GetComponent<Enemy>().isClosest = true;

        if((closestEnemie.transform.position - transform.position).magnitude < distanceBeforeDeath && !die)
        {
            die = true;
            animator.SetTrigger("die");
            for (int i = 0; i < FindObjectsOfType<Enemy>().Length; i++)
            {
                FindObjectsOfType<Enemy>()[i].GetComponent<Animator>().SetTrigger("Yippie");
            }
            for (int i = 0; i < keys.Count; i++)
            {
                keys[i].GetComponent<Rigidbody>().useGravity = true;
            }

            GetComponent<AudioSource>().Play();

            deadScreen.SetActive(true);
        } 
        if(die)
        {
            if (Camera.main.GetComponent<AudioLowPassFilter>().cutoffFrequency > 500)
            {
                Camera.main.GetComponent<AudioLowPassFilter>().cutoffFrequency -= 10;
            }
        } else
        {
            Camera.main.GetComponent<AudioLowPassFilter>().cutoffFrequency = 10000;
        }

        if(Input.GetKey(KeyCode.Escape))
        {
            Instantiate(boem,transform.position, Quaternion.identity);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "key")
        {
            keys.Add(other.gameObject);
        }
    }

    public void TryToOpenDoor(GameObject door)
    {
        if (keys.Count > 0)
        {
            keys[keys.Count - 1].GetComponent<Rigidbody>().useGravity = true;
            keys[keys.Count - 1].GetComponent<BoxCollider>().enabled = false;
            keys.RemoveAt(keys.Count - 1);
            door.GetComponent<door>().Unlock();
        }
    }
}
