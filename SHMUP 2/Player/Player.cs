using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Shooting
{
    [SerializeField] private float playerSpeed;

    [SerializeField] private float invincibilityTime;
    public bool invincibility = false;

    [SerializeField] private float screenBounds;

    public bool dead = false;

    public int score;

    public int decendt = 2;
    public bool shooting = false;

    public PlayerControler playerControler;
    private void Start()
    {
        playerControler = new PlayerControler();
        playerControler.Game.Enable();
        playerControler.Game.Movement.performed += InputMovePlayer;
        playerControler.Game.Shoot.performed += InputShoot;
        playerControler.Game.Pause.performed += Pause;
        playerControler.Game.Special.performed += SpecialAttack;
    }

    private void Update()
    {
        if(Mathf.Abs(transform.position.y + -transform.up.x * Time.deltaTime * playerSpeed) < screenBounds || dead)
        {
            transform.position += new Vector3(0, -transform.up.x * Time.deltaTime * playerSpeed, 0);
        }
        transform.up += new Vector3((0.0f - transform.up.x) / 500.0f * decendt, 0, 0) * Time.deltaTime * 200.0f;

        if(dead)
        {
            shooting = true;
            transform.up += new Vector3((2.5f - transform.up.x) / 750.0f, 0, 0) * Time.deltaTime * 200.0f;
        }
    }

    public override void Damage(int damage)
    {
        if(!invincibility)
        {
            base.Damage(damage);
            StartCoroutine(Invincibility());
        }
    }
    private IEnumerator Invincibility()
    {
        invincibility = true;
        for(int i = 0; i < 4; i++)
        {
            transform.GetChild(0).gameObject.SetActive(false);
            yield return new WaitForSeconds(invincibilityTime / 8);
            transform.GetChild(0).gameObject.SetActive(true);
            yield return new WaitForSeconds(invincibilityTime / 8);
        }
        invincibility = false;
    }

    public override void OnDeath()
    {
        playerControler.Game.Disable();
        dead = true;
        StartCoroutine(FindFirstObjectByType<SpecialScreenManager>().ShowScreen(0, 2));
    }

    public void AddScore(int scoreToAdd)
    {
        StartCoroutine(AddScoreOutPut(scoreToAdd));
    }
    private IEnumerator AddScoreOutPut(int scoreToAdd)
    {
        for(int i = 0; i < scoreToAdd; i++)
        {
            score++;
            yield return null;
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
            transform.up += new Vector3((0.8f * 1.5f * -obj.ReadValue<Vector2>().y - transform.up.x) / 500.0f * Time.deltaTime * 200.0f, 0, 0);
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
            Shoot(shootingPosition);
            yield return new WaitForSeconds(coolDown);
            yield return null;
        }
        shooting = false;
    }

    private void Pause(InputAction.CallbackContext obj)
    {
        if (!dead && !FindFirstObjectByType<WaveManager>().playerHasWon)
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
        if (playerControler == null)
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

    private void OnDisable()
    {
        playerControler.Disable();
    }
}
