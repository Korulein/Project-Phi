using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;


//This script is used to clear the scene before starting the next
public class LoadingScreenManager : MonoBehaviour
{
    public string nextSceneName;
    public float waitingTime = 5f; 

    void Start()
    {
        DestroyAllDontDestroyOnLoadObjects();
        StartCoroutine(LoadNextScene());
    }

    private void DestroyAllDontDestroyOnLoadObjects()
    {
        // Create a temporary object to get the DontDestroyOnLoad scene
        GameObject temp = new GameObject("Temp");
        DontDestroyOnLoad(temp);
        Scene ddolScene = temp.scene;

        // Find all root objects in the DontDestroyOnLoad scene
        var ddolRoots = ddolScene.GetRootGameObjects();
        foreach (var obj in ddolRoots)
        {
            if (obj != temp)
                Destroy(obj);
        }
        Destroy(temp);
    }

    private IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(waitingTime); // Add a brief delay before loading the next scene
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextSceneName);
        while (!asyncLoad.isDone)
            yield return null;
    }
}