using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class uiAnimations : MonoBehaviour
{
    [SerializeField] private string animation;
    [SerializeField] private float speed;

    [SerializeField] public float intensity;

    [SerializeField] private List<GameObject> objectsOut;
    [SerializeField] private List<GameObject> objectsIn;

    [SerializeField] private GameObject noPressButton;

    private TextMeshProUGUI myText;
    private float timer;

    private void Start()
    {
        if (animation == "game over text" || animation == "game over button")
        {
            StartCoroutine(fadeText(100, 0, 1));
        }
    }

    private void FixedUpdate()
    {
        timer += speed;
        if (animation == "game over text")
        {
            RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
            Vector3 startPosistion = rectTransform.position;
            rectTransform.position = new Vector3(startPosistion.x, Mathf.Sin(timer) * intensity + startPosistion.y, startPosistion.z);
        }
    }

    private IEnumerator fadeText(float fadeTime, float startFade, float direction)
    {
        myText = GetComponent<TextMeshProUGUI>();
        myText.color = new Color(myText.color.r, myText.color.g, myText.color.b, startFade);
        for (float i = 0; i < fadeTime; i+= 1)
        {
            myText = GetComponent<TextMeshProUGUI>();
            myText.color = new Color(myText.color.r, myText.color.g, myText.color.b, myText.color.a + (1 * direction) / fadeTime);
            yield return new WaitForSeconds(0.02f);
        }
    }

    private IEnumerator fadeText2(float fadeTime, float startFade, float direction, GameObject animatedObject)
    {
        myText = animatedObject.GetComponent<TextMeshProUGUI>();
        myText.color = new Color(myText.color.r, myText.color.g, myText.color.b, startFade);
        for (float i = 0; i < fadeTime; i += 1)
        {
            myText = animatedObject.GetComponent<TextMeshProUGUI>();
            myText.color = new Color(myText.color.r, myText.color.g, myText.color.b, myText.color.a + (1 * direction) / fadeTime);
            yield return new WaitForSeconds(0.02f);
        }
    }

    private IEnumerator fadeText3(float fadeTime, float startFade, float direction, GameObject animatedObject)
    {
        noPressButton.SetActive(true);
        Image objectImage = animatedObject.GetComponent<Image>();
        objectImage.color = new Color(objectImage.color.r, objectImage.color.g, objectImage.color.b, startFade);
        for (float i = 0; i < fadeTime; i += 1)
        {
            objectImage = animatedObject.GetComponent<Image>();
            objectImage.color = new Color(objectImage.color.r, objectImage.color.g, objectImage.color.b, objectImage.color.a + (1 * direction) / fadeTime);
            yield return new WaitForSeconds(0.02f);
        }
        transform.parent.gameObject.SetActive(false);
        noPressButton.SetActive(false);
    }

    public void animationOnButtonPress(string objectAnimation)
    {
        if (objectAnimation == "fade in")
        {
            for (int i = 0; i < objectsIn.Count; i++)
            {
                if(objectsIn[i].tag != "text")
                {
                    StartCoroutine(fadeText3(100, -0.75f, 1, objectsIn[i].transform.parent.gameObject));
                }
                StartCoroutine(fadeText2(100, 0, 1, objectsIn[i]));
            }
        }

        if (objectAnimation == "fade out")
        {
            for (int i = 0; i < objectsOut.Count; i++)
            {
                if (objectsOut[i].tag != "text")
                {
                    StartCoroutine(fadeText3(100, 0.25f, -1, objectsOut[i].transform.parent.gameObject));
                }
                StartCoroutine(fadeText2(100, 1, -1, objectsOut[i]));
            }
        }
    }
}
