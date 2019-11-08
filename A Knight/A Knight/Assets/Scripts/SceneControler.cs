using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControler : MonoBehaviour
{
    public float timeDelay = 0f; 

    private bool canChange = false;
    private float counter;
    private string sceneName;
    void Update()
    {
        counter += Time.fixedDeltaTime;
        if (canChange && counter >= 0f)
            SceneManager.LoadScene(sceneName);
    }

    public void SceneChange(string sceneName)
    {
        this.sceneName = sceneName;
        canChange = true;
        counter = -timeDelay;
    }
}
