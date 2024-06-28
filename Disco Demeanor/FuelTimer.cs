using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FuelTimer : MonoBehaviour
{
    private GameObject escapeSequenceManager;

    private void Update()
    {
        escapeSequenceManager = GameObject.FindGameObjectWithTag("EscapeSequenceManager");
        if (escapeSequenceManager != null)
        {
            if(escapeSequenceManager.GetComponent<EscapeSequenceManager>().playerIsEscaping)
            {
                float minutes = Mathf.Floor(escapeSequenceManager.GetComponent<EscapeSequenceManager>().timer / 60);
                float tenSeconds = Mathf.Floor((escapeSequenceManager.GetComponent<EscapeSequenceManager>().timer - minutes * 60) / 10);
                float seconds = Mathf.Floor(escapeSequenceManager.GetComponent<EscapeSequenceManager>().timer - minutes * 60 - tenSeconds * 10);
                GetComponent<TextMeshProUGUI>().text = "ESCAPE:\n" + minutes + ":" + tenSeconds + seconds;
            } else
            {
                GetComponent<TextMeshProUGUI>().text = "";
            }
            if(escapeSequenceManager.GetComponent<EscapeSequenceManager>().timer <= 0 && escapeSequenceManager.GetComponent<EscapeSequenceManager>().playerIsEscaping)
            {
                GetComponent<TextMeshProUGUI>().text = "ESCAPE:\n" + "0:00";
            }
        } else
        {
            GetComponent<TextMeshProUGUI>().text = "";
        }
    }
}
