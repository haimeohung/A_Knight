using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneController : MonoBehaviour
{
    public static TipList tips;

    protected static string _sceneName = "", _nextSceneName = "";
    /// <summary>
    /// Change to sceneName with name display is nextSceneName
    /// </summary>
    /// <param name="sceneName"> "WorldMap" if end playable scene </param>
    /// <param name="nextSceneName"> no need after all </param>
    public static void ChangeScene(string sceneName, string nextSceneName)
    {
        if (sceneName == "WorldMap")
            SceneManager.LoadScene(sceneName);
        else
        {
            SceneManager.LoadScene("Loading");
            _nextSceneName = nextSceneName;
            _sceneName = sceneName;
        }
    }
    public static AsyncOperation ReLoadScene() => SceneManager.LoadSceneAsync(currentSceneName);
    public static string currentSceneName => SceneManager.GetActiveScene().name;
    protected AsyncOperation ChangeScene(string sceneName) => SceneManager.LoadSceneAsync(sceneName);

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {

        }
    }
}