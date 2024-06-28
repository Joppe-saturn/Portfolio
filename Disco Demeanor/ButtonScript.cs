using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Losecondition;

public class ButtonScript : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject fuel;
    [SerializeField] private GameObject boss;
    [SerializeField] private GameObject escapeSequenceManager;
    [SerializeField] private GameObject assaultRiflePickup;

    private Vector3 playerStartPos;

    private void Awake()
    {
        if (player == null) player = GameObject.FindGameObjectWithTag("Player");
        if (escapeSequenceManager == null) escapeSequenceManager = GameObject.FindGameObjectWithTag("EscapeSequenceManager");
        if (player != null) playerStartPos = player.transform.position;
        if (assaultRiflePickup == null) assaultRiflePickup = GameObject.Find("Gun Pickup");
    }

    private void Update()
    {
        if (boss == null) boss = GameObject.FindGameObjectWithTag("Boss");
        if (fuel == null) fuel = GameObject.FindGameObjectWithTag("Fuel");
    }

    public void Retry()
    {
        //this one resets everything (I know it's messy)
        if(GetComponent<UnityEngine.UI.Image>().color.a > 0.1f)
        {
            Debug.Log(player.transform.position);
            player.GetComponent<CharacterController>().enabled = false;
            player.transform.position = playerStartPos;
            player.GetComponent<CharacterController>().enabled = true;
            Debug.Log(player.transform.position);
            player.GetComponent<HealthManager>().health = player.GetComponent<HealthManager>().defaultHealth;
            player.GetComponent<MovementTest>().enabled = true;
            player.GetComponent<LookAt>().enabled = true;
            player.GetComponent<Shooting>().enabled = true;
            if (player.GetComponent<Losecondition>().conditionState == ConditionState.Won) player.GetComponent<Losecondition>().amountOfDeaths = 0;
            player.GetComponent<Losecondition>().conditionState = ConditionState.Playing;
            if(transform.parent.GetComponent<GameOverScreen>() != null) transform.parent.GetComponent<GameOverScreen>().RemoveScreen();
            if(transform.parent.GetComponent<WinScreen>() != null) transform.parent.GetComponent<WinScreen>().RemoveScreen();
            Destroy(fuel);

            if(transform.parent.parent.GetComponent<UnHideScreens>() != null) transform.parent.parent.GetComponent<UnHideScreens>().RetsetScreens();
            if(boss != null)
            {
                boss.GetComponent<HealthManager>().health = boss.GetComponent<HealthManager>().defaultHealth;
            }
            for(int i = 0; i < escapeSequenceManager.GetComponent<EscapeSequenceManager>().spawnedEscapeEnemies.Count; i++)
            {
                Destroy(escapeSequenceManager.GetComponent<EscapeSequenceManager>().spawnedEscapeEnemies[i]);
            }
            escapeSequenceManager.GetComponent<EscapeSequenceManager>().spawnedEscapeEnemies.Clear();
            escapeSequenceManager.GetComponent<EscapeSequenceManager>().playerIsEscaping = false;
            escapeSequenceManager.GetComponent<EscapeSequenceManager>().timer = 0;
            FindObjectOfType<PlayerUi>().timer = 0;

            for(int i = 0; i < FindObjectsOfType<Bulletmove>().Length; i++)
            {
                Destroy(FindObjectsOfType<Bulletmove>()[i].gameObject);
            }

            FindObjectOfType<PlayerUi>().GetComponent<PlayerUi>().UpdateHealth();
            player.GetComponent<HealthManager>().RemoveInvinsibility();
            player.GetComponent<Rigidbody>().isKinematic = false;
            player.GetComponent<Shooting>().gunInventory.Clear();
            player.GetComponent<Shooting>().AddNewGun(player.GetComponent<Shooting>().starterGun);
            for(int i = 0;i < FindObjectsOfType<RoomManager>().Length; i++)
            {
                FindObjectsOfType<RoomManager>()[i].ResetRooms();
            }
            UIManager.instance.ToggleBossHP(false);
            assaultRiflePickup.SetActive(true);
            Unpause();
        }
    }

    public void Unpause()
    {
        Debug.Log(1);
        Time.timeScale = 1;
        if(transform.parent.parent.GetComponent<PlayerUi>() != null)
        {
            transform.parent.parent.GetComponent<PlayerUi>().Unpause();
        }
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void GoToScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}
