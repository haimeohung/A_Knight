using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneController : MonoBehaviour
{
    public static TipList tips;

    protected static string _sceneName = "", _nextSceneName = "";
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
}