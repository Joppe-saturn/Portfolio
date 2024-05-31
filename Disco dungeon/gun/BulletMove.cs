using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BulletMove : MonoBehaviour
{
    [SerializeField] private float srinkSpeed;

    private Camera cam;

    [SerializeField] private GameObject boem;

    private bool hasHitTarget = false;
    private List<GameObject> hitNotTargets = new List<GameObject>();

    [SerializeField] private GameObject rayObj;
    private GameObject player;
    private float distanceToPlayerPerWall;
    private GameObject closestObject;

    private bool canHit = false;


    private void Start()
    {
        StartCoroutine(checkPosition());
    }

    private IEnumerator checkPosition()
    {
        player = FindAnyObjectByType<Playermovement>().gameObject;
        cam = Camera.main;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        hitNotTargets.Clear();
        while (!hasHitTarget)
        {
            GetComponent<Collider>().enabled = false;
            bool whenHitSomething = Physics.Raycast(ray, out hit);
            if (whenHitSomething && hit.collider.GetComponent<MeshRenderer>().isVisible)
            {
                transform.position = hit.point;
                if (hit.collider.gameObject.tag != "Enemy")
                {
                    transform.position += new Vector3(0, 0.5f, 0);
                }
                else
                {
                    transform.position -= new Vector3(0, 0.5f, 0);
                }
                hasHitTarget = true;
            }
            else
            {
                hitNotTargets.Add(hit.collider.gameObject);
                hit.collider.gameObject.SetActive(false);
            }
        }
        GetComponent<Collider>().enabled = true;

        for (int i = 0; i < hitNotTargets.Count; i++)
        {
            hitNotTargets[i].SetActive(true);
        }

        GameObject currentRay = Instantiate(rayObj, transform.position, Quaternion.identity);
        currentRay.transform.position -= new Vector3(0, 0.5f, 0);
        float distanceBetweenPlayer = Mathf.Sqrt(Mathf.Pow(transform.position.x - player.transform.position.x, 2) + Mathf.Pow(transform.position.z - player.transform.position.z, 2));
        currentRay.transform.localScale = new Vector3(0.5f, 0.5f, distanceBetweenPlayer);
        currentRay.transform.LookAt(player.transform.position);
        currentRay.transform.position -= new Vector3((transform.position.x - player.transform.position.x) / 2, 0, (transform.position.z - player.transform.position.z) / 2);
        currentRay.GetComponent<MeshRenderer>().enabled = false;

        while (!currentRay.GetComponent<RayCastBullet>().isDone)
        {
            yield return null;
        }
        currentRay.GetComponent<BoxCollider>().enabled = false;
        
        if (currentRay.GetComponent<RayCastBullet>().walls.Count > 0)
        {
            for (int i = 0; i < currentRay.GetComponent<RayCastBullet>().walls.Count; i++)
            {
                GameObject currentWall = currentRay.GetComponent<RayCastBullet>().walls[i];
                if(distanceToPlayerPerWall == 0)
                {
                    closestObject = currentWall;
                    distanceToPlayerPerWall = Mathf.Sqrt(Mathf.Pow(Mathf.Sqrt(Mathf.Pow(currentWall.transform.position.x, 2) + Mathf.Pow(currentWall.transform.position.z, 2)) - Mathf.Sqrt(Mathf.Pow(player.transform.position.x, 2) + Mathf.Pow(player.transform.position.z, 2)), 2));
                }
                else if (Mathf.Sqrt(Mathf.Pow(Mathf.Sqrt(Mathf.Pow(currentWall.transform.position.x, 2) + Mathf.Pow(currentWall.transform.position.z, 2)) - Mathf.Sqrt(Mathf.Pow(player.transform.position.x, 2) + Mathf.Pow(player.transform.position.z, 2)), 2)) < distanceToPlayerPerWall)
                {
                    closestObject = currentWall;
                    distanceToPlayerPerWall = Mathf.Sqrt(Mathf.Pow(Mathf.Sqrt(Mathf.Pow(currentWall.transform.position.x, 2) + Mathf.Pow(currentWall.transform.position.z, 2)) - Mathf.Sqrt(Mathf.Pow(player.transform.position.x, 2) + Mathf.Pow(player.transform.position.z, 2)), 2));
                }
            }
            transform.position = closestObject.transform.position;
            transform.position -= (transform.position - player.transform.position).normalized;
            transform.position += new Vector3(0, 0.5f, 0);
        }

        Instantiate(boem, transform.position, Quaternion.identity);

        currentRay.transform.position = transform.position;
        float distanceBetweenPlayer2 = Mathf.Sqrt(Mathf.Pow(transform.position.x - player.transform.position.x, 2) + Mathf.Pow(transform.position.z - player.transform.position.z, 2));
        currentRay.transform.localScale = new Vector3(0.5f, 0.5f, distanceBetweenPlayer2);
        currentRay.transform.LookAt(player.transform.position);
        currentRay.transform.position -= new Vector3((transform.position.x - player.transform.position.x) / 2, 0, (transform.position.z - player.transform.position.z) / 2);
        currentRay.GetComponent<RayCastBullet>().isShrinking = true;
        currentRay.GetComponent<MeshRenderer>().enabled = true;
        
        canHit = true;
        StartCoroutine(shrink());
    }

    private IEnumerator shrink()
    {
        while (transform.localScale.z > 0.1)
        {
            transform.localScale -= new Vector3(srinkSpeed, srinkSpeed, srinkSpeed);
            yield return new WaitForSeconds(0.1f);
        }
        Destroy(gameObject);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Enemy" && canHit)
        {
            Destroy(other.gameObject);
        }
        if (other.GetComponent<Breakable>() != null && canHit)
        {
            other.GetComponent<Breakable>().Break();
        }
    }
}
