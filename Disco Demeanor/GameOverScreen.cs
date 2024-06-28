using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static Losecondition;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] private float timeBeforeShowUp = 3f;
    [SerializeField] private float imageTransparent = 0.7f;
    [SerializeField] private float transparantIncrease = 0.001f;

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject deathCount;

    private Image image;
    private TextMeshProUGUI text;
    private float timer;
    private List<GameObject> buttons = new List<GameObject>();

    private void Start()
    {
        //this one gets all the components for the varibles
        if (player == null) player = GameObject.FindGameObjectWithTag("Player");
        image = transform.GetChild(0).GetComponent<Image>();
        text = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        for (int i = 2; i < transform.childCount; i++)
        {
            buttons.Add(transform.GetChild(i).gameObject);
        }
        RemoveScreen();
    }

    private void Update()
    {
        //this one checks if you have lost and then lets the loose screen fade in.
        if (player.GetComponent<Losecondition>().conditionState == ConditionState.Lost)
        {
            timer += Time.deltaTime;
            if (timer > timeBeforeShowUp)
            {
                if (image.color.a < imageTransparent)
                {
                    image.color += new Color(0, 0, 0, transparantIncrease);
                }
                if(text.faceColor.a < 255)
                {
                    text.faceColor += new Color(0, 0, 0, transparantIncrease * 10);
                }
                for (int i = 2; i < buttons.Count + 2; i++)
                {
                    Image tempColor = transform.GetChild(i).GetComponent<Image>();
                    tempColor.color += new Color(0, 0, 0, transparantIncrease);
                    TextMeshProUGUI tempColorTheSecond = tempColor.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                    tempColorTheSecond.faceColor += new Color(0, 0, 0, transparantIncrease * 10);
                }
                /*deathCount.SetActive(true);    
                deathCount.GetComponent<TextMeshProUGUI>().text = "You've died: " + player.GetComponent<Losecondition>().amountOfDeaths.ToString() + "times!";*/
            }
        } 

        //this one deactivate itself if the win screen is up
        if (player.GetComponent<Losecondition>().conditionState == ConditionState.Won)
        {
            gameObject.SetActive(false);
        }

    }

    public void RemoveScreen()
    {
        //this one removes the screen
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
        text.faceColor = new Color(text.color.r, text.color.g, text.color.b, 0);
        for (int i = 2; i < buttons.Count + 2; i++)
        {
            Image tempColor = transform.GetChild(i).GetComponent<Image>();
            tempColor.color = new Color(tempColor.color.r, tempColor.color.g, tempColor.color.b, 0);
            TextMeshProUGUI tempColorTheSecond = tempColor.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            tempColorTheSecond.faceColor = new Color(tempColorTheSecond.color.r, tempColorTheSecond.color.g, tempColorTheSecond.color.b, 0);
        }
        timer = 0;
    }
}
