using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishBulletRotate : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(Rotate());
    }
    private IEnumerator Rotate()
    {
        while(true)
        {
            transform.Rotate(0, 0, 2.0f);
            yield return new WaitForSeconds(0.02f);
        }
    }
}
