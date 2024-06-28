using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Losecondition : MonoBehaviour
{
    public int amountOfDeaths = 0;
    private bool firstTimeDying;

    public enum ConditionState
    {
        Playing,
        Won,
        Lost
    }

    public ConditionState conditionState = ConditionState.Playing;

    private void Update()
    {
        //this one checks if you have lost
        if(((GetComponent<HealthManager>().health <= 0 && conditionState == ConditionState.Playing) || conditionState == ConditionState.Lost) && !firstTimeDying)
        {
            GetComponent<HealthManager>().health = 0;
            GetComponent<CharacterController>().enabled = false;
            GetComponent<MovementTest>().enabled = false;
            GetComponent<LookAt>().enabled = false;
            GetComponent<Shooting>().enabled = false;
            amountOfDeaths++;
            conditionState = ConditionState.Lost;
            firstTimeDying = true;
        }
        if(conditionState != ConditionState.Playing) 
        {
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            for(int i = 0; i < transform.GetChild(0).childCount; i++)
            {
                if(transform.GetChild(0).GetChild(i).GetComponent<MeshRenderer>() != null)
                {
                    transform.GetChild(0).GetChild(i).GetComponent<MeshRenderer>().enabled = true;
                }
            }
        }
        else
        {
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
            firstTimeDying = false;
        }
    }
}
