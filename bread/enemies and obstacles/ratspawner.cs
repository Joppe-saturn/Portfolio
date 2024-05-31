using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ratspawner : MonoBehaviour
{
    [SerializeField] private float timeToSummonRat;
    [SerializeField] private GameObject rat;
    [SerializeField] private GameObject camera;
    private float timer;
    private float position;
    private float posX;
    private float posY;
    private float randomY;

    void Start()
    {
        timer = Random.Range(0,timeToSummonRat);
        position = 1;
        randomY = Random.Range(-3, 3);
        gameObject.SetActive(false);
    }

    void Update()
    {
        posX = position * 10 + camera.transform.position.x;
        posY = (randomY / 10) + camera.transform.position.y;
        timer += Time.deltaTime;
        if (timer > timeToSummonRat)
        {
            float rotationalY;
            if(position == 1)
            {
                rotationalY = -180;

            } else
            {
                rotationalY = 180;
            }
            transform.position = new Vector3(posX, posY, transform.position.z);
            transform.rotation = Quaternion.Euler(transform.rotation.x, rotationalY, transform.rotation.z);
            Debug.Log(Quaternion.Euler(transform.rotation.x, rotationalY, transform.position.z));
            Instantiate(rat, transform.position, Quaternion.Euler(transform.rotation.x, rotationalY, transform.position.z));
            timer = Random.Range(0, timeToSummonRat);
            position *= -1;
            randomY = Random.Range(-30, 30);
        }
    }
}
