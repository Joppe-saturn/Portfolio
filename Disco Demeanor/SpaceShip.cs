using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Losecondition;

public class SpaceShip : MonoBehaviour
{
    [SerializeField] private GameObject fuel;
    [SerializeField] private GameObject escapeSequenceManager;

    private void Update()
    {
        if (fuel == null) fuel = GameObject.FindGameObjectWithTag("Fuel");
        if (escapeSequenceManager == null) escapeSequenceManager = GameObject.FindGameObjectWithTag("EscapeSequenceManager");
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player") && escapeSequenceManager.GetComponent<EscapeSequenceManager>().playerIsEscaping)
        {
            other.gameObject.GetComponent<Losecondition>().conditionState = ConditionState.Won;
            other.gameObject.GetComponent<MovementTest>().enabled = false;
            other.gameObject.GetComponent<LookAt>().enabled = false;
            other.gameObject.GetComponent<Shooting>().enabled = false;
            other.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            other.gameObject.GetComponent<CapsuleCollider>().enabled = false;
        }
    }
}
