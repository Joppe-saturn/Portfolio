using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class BulletParticleDissapear : MonoBehaviour
{
    [SerializeField] private float dissapearTime = 5f;
    [SerializeField] private float moveBack = 1f;
    public void destroyMyself()
    {
        transform.position -= transform.forward * moveBack;
        transform.GetChild(2).gameObject.SetActive(true);
        transform.GetChild(1).GetComponent<VisualEffect>().SendEvent("StopEvent");
        Destroy(gameObject,dissapearTime);
    }
}
