using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tab2PlayControler : MonoBehaviour
{
    public Animator animator;

    private bool isSwapScene = false;
    private float dt = 0f;

    private void Update()
    {
        if (isSwapScene)
        {
            dt += Time.fixedDeltaTime;
            if (dt >= 2.5f)
                SceneManager.LoadScene("SelectGamePlayScene");
        }
    }
    public void FirstTabTrigger()
    {
        animator.SetBool("FirstTab", true);
    }

    public void SelectedTrigger()
    {
        animator.SetBool("EndScene", true);
        isSwapScene = true;
    }
}
