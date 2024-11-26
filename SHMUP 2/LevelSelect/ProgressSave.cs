using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ProgressSave : MonoBehaviour
{
    [SerializeField] private List<GameObject> levels;
    [SerializeField] private List<string> levelName;
    [SerializeField] private List<Material> materials;

    private int CurrentSelectedLevel;

    private void Start()
    {
        for (int i = 0; i < levels.Count; i++)
        {
            string currentLevel = "Level " + (i + 1);
            levels[i].GetComponent<MeshRenderer>().material = materials[PlayerPrefs.GetInt(currentLevel)];
        }
        CurrentSelectedLevel = 0;
    }

    private void Update()
    {
        transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = levelName[CurrentSelectedLevel];
        transform.GetChild(0).transform.position = levels[CurrentSelectedLevel].transform.position - new Vector3(0, 0, 50);
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (CurrentSelectedLevel < levels.Count - 1 && PlayerPrefs.GetInt("Level " + (CurrentSelectedLevel + 2)) > 0)
            {
                CurrentSelectedLevel++;
            }
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (CurrentSelectedLevel > 0)
            {
                CurrentSelectedLevel--;
            }
        }
    }

    public void EnterLevel()
    {
        SceneManager.LoadScene("Level " + (CurrentSelectedLevel + 1));
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("TitleScreen");
    }
}
