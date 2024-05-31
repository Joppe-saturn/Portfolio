using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    [SerializeField] private GameObject ray;

    [SerializeField] private GameObject reserve;

    private List<float> levelBpm = new List<float>();
    private int level;
    private float timer;

    private GameObject player;

    private int canMove = 0;

    private Vector3 moveTo;

    private void Start()
    {
        levelBpm = LevelBuilder.instance.levelbpm;
        level = LevelBuilder.instance.level;
        player = FindAnyObjectByType<Playermovement>().gameObject;
    }

    private void FixedUpdate()
    {
        timer++;
        if(timer > 50 / (levelBpm[level] / 60)) 
        {
            if (LevelBuilder.instance.hasGameStartedYet) 
            {
                StartCoroutine(checkIfCanMove());
                canMove = canMove - (2 * canMove) + 1;
            }
            timer = 0;
        }
    }

    private IEnumerator checkIfCanMove()
    {
        GameObject currentRay = Instantiate(ray, transform.position, Quaternion.identity);
        float distanceBetweenPlayer = Mathf.Sqrt(Mathf.Pow(transform.position.x - player.transform.position.x, 2) + Mathf.Pow(transform.position.z - player.transform.position.z, 2));
        currentRay.transform.localScale = new Vector3(0.5f, 0.5f, distanceBetweenPlayer);
        currentRay.transform.LookAt(player.transform.position);
        currentRay.transform.position -= new Vector3((transform.position.x - player.transform.position.x) / 2, 0, (transform.position.z - player.transform.position.z) / 2);
        yield return null;
        if (!currentRay.GetComponent<RayCastFindPlayer>().hasHitWall && canMove == 0)
        {
            float moveToX = player.transform.position.x - transform.position.x;
            float moveToZ = player.transform.position.z - transform.position.z;
            
            moveTo = transform.position;

            if (Mathf.Sqrt(Mathf.Pow(moveToX, 2)) > Mathf.Sqrt(Mathf.Pow(moveToZ, 2)))
            {
                if (moveToX > 0)
                {
                    moveTo += new Vector3(1, 0, 0);
                }
                else
                {
                    moveTo -= new Vector3(1, 0, 0);
                }
            }
            else
            {
                if (moveToZ > 0)
                {
                    moveTo += new Vector3(0, 0, 1);
                }
                else
                {
                    moveTo += new Vector3(0, 0, -1);
                }
            }

            Collider[] colliders = Physics.OverlapBox(moveTo, new Vector3(0.1f, 3, 0.1f));
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].tag == "wall" || colliders[i].tag == "Tresure" || colliders[i].tag == "Enemy" || colliders[i].tag == "WhiteTile")
                {
                    moveTo = transform.position; break;
                }
            }
            GameObject spot = Instantiate(reserve, moveTo, Quaternion.identity);
            yield return null;
            Collider[] colliders2 = Physics.OverlapBox(moveTo, Vector3.one * 0.1f);
            int howManyColliders = 0;
            for (int i = 0; i < colliders2.Length; i++)
            {
                if (colliders2[i].tag == "reserveSpot")
                {
                    howManyColliders++;
                    if(howManyColliders > 1)
                    {
                        DestroyImmediate(colliders2[i].gameObject);
                        howManyColliders--;
                    }
                }
            }
            yield return null;
            if(spot.gameObject != null)
            {
                transform.position = moveTo;
                transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
            }
            Destroy(spot);
        }
        Destroy(currentRay);
    }
}
