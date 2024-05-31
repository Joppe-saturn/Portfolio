using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class tips : MonoBehaviour
{
    [SerializeField] private List<string> Tips = new List<string>();
    private void Update()
    {
        if(gameObject.GetComponent<TextMeshProUGUI>().color.a < 0.2)
        {
            gameObject.GetComponent<TextMeshProUGUI>().text = Tips[Random.Range(0, Tips.Count)];
        }
    }
}
