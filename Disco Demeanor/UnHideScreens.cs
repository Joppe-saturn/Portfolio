using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnHideScreens : MonoBehaviour
{
    //this script unhides the win and loose screen. This is just a quality of life script.
    private void Start()
    {
        RetsetScreens();
    }

    public void RetsetScreens()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }
    }
}
