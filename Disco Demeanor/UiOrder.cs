using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiOrder : MonoBehaviour
{
    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if(player.GetComponent<Losecondition>().conditionState == Losecondition.ConditionState.Playing)
        {
            GetComponent<Canvas>().sortingOrder = -1;
        } else
        {
            GetComponent<Canvas>().sortingOrder = 1;
        }
    }
}
