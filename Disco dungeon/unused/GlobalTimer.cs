using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalTimer : MonoBehaviour
{
    public float globalTimer;

    private void FixedUpdate()
    {
        globalTimer += 1;
    }
}
