using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using static Losecondition;

public class Fuel : MonoBehaviour
{

    public bool hasBeenCollected;

    private GameObject escapeSequenceManager;
    
    public Vector3 startPos;

    [SerializeField] private GameObject audioManager;
    [SerializeField] private List<AudioClip> audioClips = new List<AudioClip>();

    private void Start()
    {
        startPos = transform.position;
        escapeSequenceManager = GameObject.FindGameObjectWithTag("EscapeSequenceManager");
    }

    private void OnTriggerEnter(Collider other)
    {
        // Starts the escape sequence when picked up.
        if(other.CompareTag("Player"))
        {
            escapeSequenceManager.GetComponent<EscapeSequenceManager>().StartEscapeSequence(other.gameObject);
            transform.position = new Vector3 (0, -1000, 0); //this line "deletes" the fuel. However we still need the script to be active so this will do.
            if (audioManager != null)
            {
                GameObject audioInstance = Instantiate(audioManager);
                audioInstance.GetComponent<AudioSource>().clip = audioClips[0];
                audioInstance.GetComponent<AudioSource>().volume = 0.5f;
                audioInstance.GetComponent<AudioManager>().valuesSet = true;
            }
        }
    }
}
