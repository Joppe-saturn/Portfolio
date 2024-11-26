using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpecialScreenManager : MonoBehaviour
{
    private void Start()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);
        transform.GetChild(2).gameObject.SetActive(true);
        transform.GetChild(3).gameObject.SetActive(false);
        FindFirstObjectByType<Player>().UnPauseOutput();
        FindFirstObjectByType<Player>().shooting = false;
    }

    public IEnumerator ShowScreen(int screenIndex, float time)
    {
        transform.GetChild(2).gameObject.SetActive(false);
        transform.GetChild(screenIndex).gameObject.SetActive(true);
        time /= 0.02f;
        Image image = transform.GetChild(screenIndex).GetChild(0).GetComponent<Image>();
        TextMeshProUGUI text = transform.GetChild(screenIndex).GetChild(1).GetComponent<TextMeshProUGUI>();
        Image button1Image = transform.GetChild(screenIndex).GetChild(2).GetComponent<Image>();
        Image button2Image = transform.GetChild(screenIndex).GetChild(3).GetComponent<Image>();
        TextMeshProUGUI button1Text = transform.GetChild(screenIndex).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI button2Text = transform.GetChild(screenIndex).GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>();

        image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
        button1Image.color = new Color(button1Image.color.r, button1Image.color.g, button1Image.color.b, 0);
        button2Image.color = new Color(button2Image.color.r, button2Image.color.g, button2Image.color.b, 0);
        button1Text.color = new Color(button1Text.color.r, button1Text.color.g, button1Text.color.b, 0);
        button2Text.color = new Color(button2Text.color.r, button2Text.color.g, button2Text.color.b, 0);

        yield return new WaitForSeconds(1.0f);

        for (int i = 0; i < time; i++)
        {
            image.color += new Color(0, 0, 0, 0.29f / time);
            text.color += new Color(0, 0, 0, 0.47f / time);
            button1Image.color += new Color(0, 0, 0, 0.59f / time);
            button2Image.color += new Color(0, 0, 0, 0.59f / time);
            button1Text.color += new Color(0, 0, 0, 1 / time);
            button2Text.color += new Color(0, 0, 0, 1 / time);
            yield return new WaitForSeconds(0.02f);
        }
    }
}
