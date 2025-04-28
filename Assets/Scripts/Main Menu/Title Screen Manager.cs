using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenManager : MonoBehaviour
{
    public static TitleScreenManager instance { get; private set; }
    [Header("Scene Indexes")]
    //[SerializeField] int mainMenuSceneIndex = 0;
    [SerializeField] int deskSceneIndex = 1;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void StartNewGame()
    {
        StartCoroutine(LoadDeskScene());
    }
    public IEnumerator LoadDeskScene()
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(deskSceneIndex);
        yield return null;
    }
}
