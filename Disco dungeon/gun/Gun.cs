using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{
    public float energy;

    [SerializeField] private Image energyBar;

    [SerializeField] private float divideEnergy;
    [SerializeField] private float maxEnergy;
    [SerializeField] private float subEnergy;
    [SerializeField] private float canShootBeforeMax;

    private bool hasShot = false;

    [SerializeField] private GameObject bullet;

    void Update()
    {
        energyBar.transform.localScale = new Vector3(1, energy, 1);
        if (energy > maxEnergy)
        {
            energy = maxEnergy;
        }
        if (energy < 0 && !hasShot)
        {
            energy = 0;
        }
    }

    public void AddEnergy(float addedEnergy)
    {
        energy += addedEnergy / divideEnergy;
    }

    public void SubEnergy()
    {
        energy -= subEnergy;
    }

    
    public void shoot()
    {
        if (energy >= maxEnergy - canShootBeforeMax && !hasShot)
        {
            Instantiate(bullet, transform.position, transform.rotation);
            StartCoroutine(EnergyShootAnimation());
        }
    }

    private IEnumerator EnergyShootAnimation()
    {
        hasShot = true;
        while(energy > 0)
        {
            energy -= 0.1f;
            yield return null;
        }
        energy = 0;
        hasShot = false;
    }
}
