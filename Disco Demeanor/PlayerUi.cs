using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerUi : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject health;
    [SerializeField] private float spaceBetweenHealth = 35;
    private List<GameObject> healthBar = new List<GameObject>();
    public float timer;

    private void Awake()
    {
        transform.Find("Pause screen").gameObject.SetActive(true);
    }

    private void Start()
    {
        transform.Find("Pause screen").gameObject.SetActive(false);
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        UpdateHealth();
    }

    private void Update()
    {
        // Displays which gun the player is holding.
        transform.Find("Gun").GetComponent<TextMeshProUGUI>().text = player.GetComponent<Shooting>().currentlySelectedGun.gunName;
        if (player.GetComponent<Losecondition>().conditionState == Losecondition.ConditionState.Lost)
        {
            transform.Find("Gun").GetComponent<TextMeshProUGUI>().text = "";
            for (int i = 0; i < healthBar.Count; i++)
            {
                Destroy(healthBar[i].gameObject);
            }
            healthBar.Clear();
        }

        if(player.GetComponent<Losecondition>().conditionState == Losecondition.ConditionState.Playing)
        {
            timer += Time.deltaTime;
        }

        // Displays how much time the player's spent on the level.
        transform.Find("Timer").GetComponent<TextMeshProUGUI>().text = "" + Mathf.Floor(timer/3600) + ":" + (Mathf.Floor(timer / 600) - Mathf.Floor(timer / 3600) * 6) + (Mathf.Floor(timer / 60) - Mathf.Floor(timer / 600) * 10) + ":" + Mathf.Floor(Mathf.Floor(timer / 10) - (Mathf.Floor(timer / 60) * 6)) + Mathf.Floor(timer - Mathf.Floor(timer / 10) * 10);

        // Pause screen. Self explanatory.
        if (Input.GetKeyDown(KeyCode.Escape) && player.GetComponent<Losecondition>().conditionState == Losecondition.ConditionState.Playing)
        {
            if (transform.Find("Pause screen").gameObject.activeSelf)
            {
                Unpause();
            }
            else
            {
                Pause();
            }
        }
        
        // Player can only pause if they're actively playing the game.
        if(player.GetComponent<Losecondition>().conditionState != Losecondition.ConditionState.Playing)
        {
            Unpause();
        }
    }

    public void Pause()
    {
        // Pauses the game. Self explanatory.
        transform.Find("Pause screen").gameObject.SetActive(true);
        player.GetComponent<Shooting>().enabled = false;
        player.GetComponent<LookAt>().enabled = false;
        Time.timeScale = 0;
        for(int i = 0; i < FindObjectsOfType<AudioManager>().Length; i++)
        {
            Destroy(FindObjectsOfType<AudioManager>()[i]);
        }
        if(GameObject.FindGameObjectWithTag("Music") != null)
        {
            GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>().volume = 0;
        }
    }

    public void Unpause()
    {
        // Unpauses the game. Ditto.
        transform.Find("Pause screen").gameObject.SetActive(false);
        player.GetComponent<Shooting>().enabled = true;
        player.GetComponent<LookAt>().enabled = true;
        Time.timeScale = 1;
        if (GameObject.FindGameObjectWithTag("Music") != null)
        {
            GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>().volume = 1;
        }
    }

    public void UpdateHealth()
    {
        // Updates the health display. Only call this whenever the player takes damage or heals damage.
        for (int i = 0; i < healthBar.Count; i++)
        {
            Destroy(healthBar[i].gameObject);
        }
        healthBar.Clear();
        for (int i = 0; i < player.GetComponent<HealthManager>().health; i++)
        {
            float screenWith = Screen.width / 2 - player.GetComponent<HealthManager>().health * spaceBetweenHealth / 2 + spaceBetweenHealth / 2;
            healthBar.Add(Instantiate(health, new Vector3(screenWith + spaceBetweenHealth * i, 48, 0), Quaternion.identity));
            healthBar[i].transform.parent = transform.Find("HealthManager");
        }
    }
}
