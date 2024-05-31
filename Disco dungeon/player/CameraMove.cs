using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float moveOffset;

    private void Update()
    {
        if(LevelBuilder.instance.hasGameStartedYet)
        {
            Vector3 moveTo = new Vector3((player.transform.position.x + 2 - transform.position.x) / moveOffset, 0, (player.transform.position.z - transform.position.z) / moveOffset);
            transform.position += moveTo;
        } else
        {
            transform.position = new Vector3(player.transform.position.x + 2, 6, player.transform.position.z);
        }
    }
}
