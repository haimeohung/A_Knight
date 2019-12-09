using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixPlayerAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    [Range(0f,3f)][SerializeField] private float stopTime = 1f;
    [SerializeField] private bool reEnable = false;
    private Animator animator;
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("StickMan_Jumping"))
            StartCoroutine(StopAnimation());
        else
            reEnable = false;
    }

    IEnumerator StopAnimation()
    {
        float currentTime = 0f;
        while (!reEnable)
        {
            yield return null;
            currentTime += Time.deltaTime;
            if (currentTime > stopTime)
            {
                animator.enabled = false;
                reEnable = true;
            }
        }
        animator.enabled = true;
    }
}
