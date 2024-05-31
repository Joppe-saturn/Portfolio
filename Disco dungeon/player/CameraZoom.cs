using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;

public class CameraZoom : MonoBehaviour
{
    [SerializeField] private float zoomAmount;

    [SerializeField] private TextMeshProUGUI goText;

    private float timer;

    public bool canMove = false;
    private bool startCountdown = false;

    private void Start()
    {
        goText.text = "";
    }

    private void FixedUpdate()
    {
        if(canMove)
        {
            timer++;
            if (timer > 50 / (LevelBuilder.instance.levelbpm[LevelBuilder.instance.level] / 60))
            {
                gameObject.GetComponent<Camera>().fieldOfView -= zoomAmount;
                timer = 0;
            }

            if (gameObject.GetComponent<Camera>().fieldOfView < 60)
            {
                gameObject.GetComponent<Camera>().fieldOfView += zoomAmount / (50 / (LevelBuilder.instance.levelbpm[LevelBuilder.instance.level] / 60));
            }
        } else
        {
            timer++;
            if (timer > 50 / (LevelBuilder.instance.levelbpm[LevelBuilder.instance.level] / 60))
            {
                timer = 0;
            }
        }

        if(!startCountdown && canMove)
        {
            StartCoroutine(GoTextAppear());
        }
    }

    private IEnumerator GoTextAppear()
    {
        if(timer == 0)
        {
            startCountdown = true;
            goText.text = "3";
            yield return new WaitForSeconds(1 / (LevelBuilder.instance.levelbpm[LevelBuilder.instance.level] / 60));
            goText.text = "2";
            yield return new WaitForSeconds(1 / (LevelBuilder.instance.levelbpm[LevelBuilder.instance.level] / 60));
            goText.text = "1";
            yield return new WaitForSeconds(1 / (LevelBuilder.instance.levelbpm[LevelBuilder.instance.level] / 60));
            goText.text = "GO!!!";
            LevelBuilder.instance.hasGameStartedYet = true;
            yield return new WaitForSeconds(1 / (LevelBuilder.instance.levelbpm[LevelBuilder.instance.level] / 60));
            goText.text = "";
        }
    }
}
