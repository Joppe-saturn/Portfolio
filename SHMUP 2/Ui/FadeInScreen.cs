using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInScreen : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(FadeOut(2.0f));
    }

    public IEnumerator FadeIn(float time)
    {
        time /= 0.02f;
        transform.GetChild(0).GetComponent<Image>().color = new Color(0, 0, 0, 0);
        for (int i = 0; i < time; i++)
        {
            transform.GetChild(0).GetComponent<Image>().color += new Color(0, 0, 0, 1 / time);
            yield return new WaitForSeconds(0.02f);
        }
    }

    public IEnumerator FadeOut(float time)
    {
        time /= 0.02f;
        transform.GetChild(0).GetComponent<Image>().color = new Color(0, 0, 0, 1);
        for (int i = 0; i < time; i++)
        {
            transform.GetChild(0).GetComponent<Image>().color += new Color(0, 0, 0, -1 / time);
            yield return new WaitForSeconds(0.02f);
        }
    }
}
