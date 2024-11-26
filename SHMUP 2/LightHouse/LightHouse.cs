using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class LightHouse : MonoBehaviour
{
    private Light lightA;
    private Light lightB;
    private bool special;
    public int health = 3;

    private void Start()
    {
        lightA = transform.GetChild(0).GetComponent<Light>();
        lightB = transform.GetChild(1).GetComponent<Light>();
        StartCoroutine(StartLightCycle());
    }

    private IEnumerator StartLightCycle()
    {
        float i = 0;
        while (true)
        {
            if(health < 1)
            {
                lightA.intensity = 0;
                lightB.intensity = 0;
                break;
            }
            if(!special)
            {
                i += 0.00125f;
                float sinResult = Mathf.Abs(Mathf.Pow(Mathf.Sin(i * 1.2f), 3f));
                lightA.intensity = sinResult * 10000.0f * health;
                lightB.intensity = sinResult * 30.0f * health;
            } else
            {
                i = 0;
            }
            yield return new WaitForSeconds(0.001f);
        }
    }

    public IEnumerator SpecialAttack()
    {
        if (!special && health > 0)
        {
            special = true;
            Volume volume = GetComponent<Volume>();
            volume.enabled = true;
            for (int i = 0; i < 100; i++)
            {
                lightA.intensity += (12000.0f - lightA.intensity) / 100.0f;
                lightB.intensity += (360.0f - lightB.intensity) / 100.0f;
                yield return new WaitForSeconds(0.02f);
            }
            BaseEnemy[] enemies = enemies = FindObjectsByType<BaseEnemy>(FindObjectsSortMode.None);
            for (int i = 0;i < enemies.Length;i++)
            {
                enemies[i].Damage(12 * health);
            }
            lightA.intensity = 0;
            lightB.intensity = 0;
            volume.enabled = false;
            yield return new WaitForSeconds(30);
            special = false;
        }
    }
}
