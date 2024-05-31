using UnityEngine;
using UnityEngine.SceneManagement;

public class audio : MonoBehaviour
{
    [SerializeField] private string[] stopAtScene;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        for(int i = 0; i < stopAtScene.Length;  i++)
        {
            if (SceneManager.GetActiveScene().name == stopAtScene[i])
            {
                gameObject.GetComponent<AudioSource>().Stop();
            }
        }
    }
}
