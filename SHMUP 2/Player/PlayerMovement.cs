using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    private GameObject player;
    public PlayerControler playerControler;

    public int decendt = 2;
    public bool shooting = false;

    [SerializeField] private bool debug;

    private void Awake()
    {
        playerControler = new PlayerControler();
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        playerControler.Game.Enable();
        playerControler.Game.Movement.performed += InputMovePlayer;
        playerControler.Game.Shoot.performed += InputShoot;
        playerControler.Game.Pause.performed += Pause;
        playerControler.Game.Special.performed += SpecialAttack;
        if (!debug)
        {
            SceneManager.LoadScene("TitleScreen");
        }
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex < 3)
        {
            playerControler.Game.Disable();
            playerControler.Pause.Disable();
        }
        if (player == null && SceneManager.GetActiveScene().buildIndex > 2)
        {
            player = FindFirstObjectByType<Player>().gameObject;
        }
    }

    private void InputMovePlayer(InputAction.CallbackContext obj)
    {
        StartCoroutine(OutputMovePlayer(obj));
    }
    private IEnumerator OutputMovePlayer(InputAction.CallbackContext obj)
    {
        while (playerControler.Game.Movement.IsPressed())
        {
            decendt = 1;
            player.transform.up += new Vector3((0.8f * 1.5f * -obj.ReadValue<Vector2>().y - player.transform.up.x) / 500.0f * Time.deltaTime * 200.0f, 0, 0);
            yield return null;
        }
        decendt = 2;
    }

    private void InputShoot(InputAction.CallbackContext obj)
    {
        if (!shooting)
        {
            StartCoroutine(OutputShoot(obj));
        }
    }
    private IEnumerator OutputShoot(InputAction.CallbackContext obj)
    {
        shooting = true;
        while (playerControler.Game.Shoot.IsPressed())
        {
            RaycastHit hit;
            Vector3 mousePos = Input.mousePosition;
            bool hasHitTarget = false;
            int emergencyBreak = 0;
            List<GameObject> deactiveObjects = new List<GameObject>();
            while (!hasHitTarget)
            {
                Ray ray = Camera.main.ScreenPointToRay(mousePos);
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.tag == "Btc")
                    {
                        hasHitTarget = true;
                        break;
                    }
                    else
                    {
                        deactiveObjects.Add(hit.collider.gameObject);
                        hit.collider.enabled = false;
                    }
                }
                mousePos += ((new Vector3(1250.0f, 300.0f, 0.0f) - mousePos) / 100.0f);
                emergencyBreak++;
                if (emergencyBreak > 250)
                {
                    break;
                }
            }
            for (int i = 0; i < deactiveObjects.Count; i++)
            {
                deactiveObjects[i].GetComponent<Collider>().enabled = true;
            }
            Vector3 shootingPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 10.0f));
            if(player != null)
            {
                player.GetComponent<Player>().Shoot(shootingPosition);
            }
            if (player != null)
            {
                yield return new WaitForSeconds(player.GetComponent<Player>().coolDown);
            }
            yield return null;
        }
        shooting = false;
    }

    private void Pause(InputAction.CallbackContext obj)
    {
        if(!player.GetComponent<Player>().dead && !FindFirstObjectByType<WaveManager>().playerHasWon)
        {
            FindFirstObjectByType<SpecialScreenManager>().transform.GetChild(3).gameObject.SetActive(true);
            MusicManager musicManager = FindFirstObjectByType<MusicManager>();
            musicManager.GetComponent<AudioSource>().pitch = 0;
            for (int i = 0; i < FindFirstObjectByType<MusicManager>().transform.childCount; i++)
            {
                musicManager.transform.GetChild(i).GetComponent<AudioSource>().pitch = 0;
            }
            musicManager.transform.GetChild(3).GetComponent<AudioSource>().pitch = 1;
            musicManager.transform.GetChild(3).GetComponent<AudioSource>().Play();
            Time.timeScale = 0;
            playerControler.Game.Disable();
            playerControler.Pause.Enable();
            playerControler.Pause.Pause.performed += UnPause;
        }
    }
    private void UnPause(InputAction.CallbackContext obj)
    {
        UnPauseOutput();
    }
    public void UnPauseOutput()
    {
        FindFirstObjectByType<SpecialScreenManager>().transform.GetChild(3).gameObject.SetActive(false);
        MusicManager musicManager = FindFirstObjectByType<MusicManager>();
        musicManager.GetComponent<AudioSource>().pitch = 1;
        for (int i = 0; i < FindFirstObjectByType<MusicManager>().transform.childCount; i++)
        {
            musicManager.transform.GetChild(i).GetComponent<AudioSource>().pitch = 1;
        }
        musicManager.transform.GetChild(3).GetComponent<AudioSource>().Stop();
        Time.timeScale = 1;
        if(playerControler == null)
        {
            playerControler = new PlayerControler();
        }
        playerControler.Game.Enable();
        playerControler.Pause.Disable();
    }

    private void SpecialAttack(InputAction.CallbackContext obj)
    {
        StartCoroutine(FindFirstObjectByType<LightHouse>().SpecialAttack());
    }

}
