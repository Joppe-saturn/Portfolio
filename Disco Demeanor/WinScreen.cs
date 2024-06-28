using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static Losecondition;

public class WinScreen : MonoBehaviour
{
    [SerializeField] private float timeBeforeShowUp = 3f;
    [SerializeField] private float imageTransparent = 1f;
    [SerializeField] private float transparantIncrease = 0.01f;
    [SerializeField] private float differenceBetweenScreen = 0f;

    [SerializeField] private GameObject player;

    private UnityEngine.UI.Image image;
    private TextMeshProUGUI text;
    private float timer;
    private List<GameObject> buttons = new List<GameObject>();

    private void Start()
    {
        //this one gets all the components for the varibles
        if (player == null) player = GameObject.FindGameObjectWithTag("Player");
        image = transform.GetChild(0).GetComponent<UnityEngine.UI.Image>();
        text = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        for(int i = 0; i < transform.childCount; i++)
        {
            if(transform.GetChild(i).GetComponent<ButtonScript>() != null)
            {
                buttons.Add(transform.GetChild(i).gameObject);
            }
        }
        RemoveScreen();
    }

    private void Update()
    {
        //this one checks if you have won and then lets the win screen fade in.
        if(player.GetComponent<Losecondition>().conditionState == ConditionState.Won)
        {
            transform.Find("DeathText").GetComponent<TextMeshProUGUI>().text = "Deaths: " + player.GetComponent<Losecondition>().amountOfDeaths.ToString();
            timer += Time.deltaTime;
            if (timer > timeBeforeShowUp)
            {
                if (image.color.a < imageTransparent)
                {
                    image.color += new Color(0, 0, 0, transparantIncrease);
                }
                if(timer > differenceBetweenScreen + timeBeforeShowUp)
                {
                    if (text.faceColor.a < 255)
                    {
                        text.faceColor += new Color(0, 0, 0, transparantIncrease * 10);
                        transform.Find("DeathText").GetComponent<TextMeshProUGUI>().faceColor += new Color(0, 0, 0, transparantIncrease * 10);
                    }
                    for (int i = 0; i < buttons.Count; i++)
                    {
                        UnityEngine.UI.Image tempColor = buttons[i].GetComponent<UnityEngine.UI.Image>();
                        tempColor.color += new Color(0, 0, 0, transparantIncrease);
                        TextMeshProUGUI tempColorTheSecond = tempColor.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                        tempColorTheSecond.faceColor += new Color(0, 0, 0, transparantIncrease * 10);
                    }
                }
            }
        }

        //this one deactivate itself if the loose screen is up
        if (player.GetComponent<Losecondition>().conditionState == ConditionState.Lost)
        {
            gameObject.SetActive(false);
        }
    }

    public void RemoveScreen()
    {
        //this one removes the screen
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
        text.faceColor = new Color(text.color.r, text.color.g, text.color.b, 0);
        transform.Find("DeathText").GetComponent<TextMeshProUGUI>().faceColor = new Color(text.color.r, text.color.g, text.color.b, 0);
        for (int i = 0; i < buttons.Count; i++)
        {
            UnityEngine.UI.Image tempColor = buttons[i].GetComponent<UnityEngine.UI.Image>();
            tempColor.color = new Color(tempColor.color.r, tempColor.color.g, tempColor.color.b, 0);
            TextMeshProUGUI tempColorTheSecond = tempColor.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            tempColorTheSecond.faceColor = new Color(tempColorTheSecond.color.r, tempColorTheSecond.color.g, tempColorTheSecond.color.b, 0);
        }
        timer = 0;
    }
}
