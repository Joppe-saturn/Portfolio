using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class LevelBuilder : MonoBehaviour
{
    public static LevelBuilder instance { get; private set; }

    [SerializeField] private List<GameObject> tilesPrefab = new List<GameObject>();
    [SerializeField] private List<GameObject> extraObjectsList = new List<GameObject>();
    [SerializeField] private float tileSize;

    [SerializeField] public int level;
    [SerializeField] LevelLayout levelLayout;

    private List<int[,]> tiles;
    private List<int[,]> extraObjects;
    private List<GameObject> placedTiles = new List<GameObject>();
    private List<GameObject> placedExtraObjects = new List<GameObject>();

    [SerializeField] public bool buildScreen = false;
    [SerializeField] public bool clearScreen = false;

    [SerializeField] public List<float> levelbpm = new List<float>();

    [SerializeField] private float wallHeight;

    private int evenCounter;

    [SerializeField] private float fallheight;
    [SerializeField] private float fallTime;
    [SerializeField] private float fallOffset;
    [SerializeField] public float fallMultiplier;

    private Camera cam;

    private float order;

    [SerializeField] private GameObject camera;

    [SerializeField] private GameObject player;
    
    public bool hasGameStartedYet = false;
    public bool hasGameEndedYet = false;

    public bool canFall = false;

    public List<GameObject> tresure = new List<GameObject>();

    private List<GameObject> breakables = new List<GameObject>();
    private GameObject tileToPlace;

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(this);
        } 
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        cam = Camera.main;
        tiles = levelLayout.getLevel();
        extraObjects = levelLayout.getObjects();
        BuildLevel();
    }

    private void BuildLevel()
    {
        evenCounter = 0;
        order = 0;
        //this one checks if the previous level was removed
        if (placedTiles.Count < 1)
        {
            for (int i = 0; i < tiles[level].GetLength(0); i++)
            {
                for (int j = 0; j < tiles[level].Length / tiles[level].GetLength(0); j++)
                {
                    //this one switches the evenCounter vairable between 0 and 1 every time it's run
                    evenCounter = evenCounter - (2 * evenCounter) + 1;
                    tileToPlace = tilesPrefab[tiles[level][i, j]];

                    //this one handles tiles that could be broken
                    if (tiles[level][i, j] == 5)
                    {
                        breakables.Add(Instantiate(tileToPlace, new Vector3((i - tiles[level].GetLength(0) / 2) * tileSize, wallHeight, (j - tiles[level].Length / tiles[level].GetLength(0) / 2) * tileSize), Quaternion.identity));
                        tileToPlace = tileToPlace.GetComponent<Breakable>().replaceMe.gameObject;
                    }
                    
                    placedTiles.Add(Instantiate(tileToPlace, new Vector3((i - tiles[level].GetLength(0) / 2) * tileSize, 0, (j - tiles[level].Length / tiles[level].GetLength(0) / 2) * tileSize), Quaternion.identity));

                    placedTiles[placedTiles.Count - 1].GetComponent<TileProperties>().materialCounter = evenCounter;

                    //this one handles tiles that can't be walked through
                    if (placedTiles[placedTiles.Count - 1].tag == "wall")
                    {
                        placedTiles[placedTiles.Count - 1].transform.localScale = new Vector3(tileSize, wallHeight, tileSize);
                    }

                    //this one handles extra objects like tresure or enemies
                    if (extraObjects[level][i, j] != 0)
                    {
                        placedExtraObjects.Add(Instantiate(extraObjectsList[extraObjects[level][i, j]], new Vector3((i - tiles[level].GetLength(0) / 2) * tileSize, 0.5f, (j - tiles[level].Length / tiles[level].GetLength(0) / 2) * tileSize), Quaternion.identity));
                        if (extraObjects[level][i, j] == 2)
                        {
                            tresure.Add(placedExtraObjects[placedExtraObjects.Count - 1].gameObject);
                        }
                    }
                }
            }
            StartCoroutine(LetTilesFall());
        }
        else
        {
            Debug.Log("There is already a level loaded");
        }
    }

    private void Update()
    {
        if (buildScreen)
        {
            BuildLevel();
            buildScreen = false;
        }

        if (clearScreen)
        {
            for (int i = 0; i < placedTiles.Count; i++)
            {
                Destroy(placedTiles[i]);
                clearScreen = false;
            }
            for (int i = 0; i < placedExtraObjects.Count; i++)
            {
                if(!hasGameEndedYet)
                {
                    Destroy(placedExtraObjects[i]);
                    clearScreen = false;
                } else if(placedExtraObjects[i].gameObject)
                {
                    if (placedExtraObjects[i].GetComponent<Rigidbody>())
                    {
                        placedExtraObjects[i].GetComponent<Rigidbody>().useGravity = true;
                    }
                }
            }
            for(int i = 0; i < breakables.Count; i++)
            {
                Destroy(breakables[i]);
            }
            placedTiles.Clear();
            placedExtraObjects.Clear();
        }

        if(hasGameStartedYet)
        {
            bool checkIfGameHasEndedYet = true;
            for (int i = 0; i < tresure.Count; i++)
            {
                if (tresure[i] != null)
                {
                    checkIfGameHasEndedYet = false;
                }
            }
            hasGameEndedYet = checkIfGameHasEndedYet;
        }

        if(hasGameEndedYet && hasGameStartedYet)
        {
            hasGameStartedYet = false;
            StartCoroutine(GameEnd());
        }
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < placedTiles.Count; i++)
        {
            placedTiles[i].gameObject.GetComponent<TileProperties>().bpm = levelbpm[level];
        }
    }

    private IEnumerator LetTilesFall()
    {
        for(int i = placedTiles.Count - 1; i > 0 ;i--)
        {
            Vector3 objectScreenPos = cam.WorldToViewportPoint(placedTiles[i].gameObject.transform.position);
            if (objectScreenPos.x > -0.1 && objectScreenPos.x < 1.1 && objectScreenPos.y > -0.1 && objectScreenPos.y < 1.15 && objectScreenPos.z > 0)
            {
                placedTiles[i].GetComponent<TileProperties>().fallHeight = fallheight * fallMultiplier;
                placedTiles[i].GetComponent<TileProperties>().timeToFall = fallTime * fallMultiplier;
                placedTiles[i].GetComponent<TileProperties>().order = order;
                placedTiles[i].GetComponent<TileProperties>().hasBeenTouched = true;
                order += fallOffset * fallMultiplier;
            }
        }
        yield return new WaitForSeconds((order - fallOffset + fallTime) * fallMultiplier);
        canFall = true;
        while (canFall)
        {
            yield return null;
        }
        camera.GetComponent<CameraZoom>().canMove = true;
    }

    private IEnumerator GameEnd()
    {
        for (int i = placedTiles.Count - 1; i > 0; i--)
        {
            Vector3 objectScreenPos = cam.WorldToViewportPoint(placedTiles[i].gameObject.transform.position);
            if (objectScreenPos.x > -0.1 && objectScreenPos.x < 1.1 && objectScreenPos.y > -0.1 && objectScreenPos.y < 1.03 && objectScreenPos.z > 0)
            {
                placedTiles[i].SetActive(false);
                yield return new WaitForSeconds(0.05f);
            } 
        }
        StartCoroutine(player.GetComponent<Playermovement>().Winner());
        clearScreen = true;
    }
}
