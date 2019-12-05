using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class UISpecialMovement : MonoBehaviour
{
    [SerializeField] private RectTransform destination;
    [SerializeField] [Range(1f, 4f)] private float timeMove = 1f;
    private RectTransform rt;
    private Vector3 delta;
    private Vector3 root;
    private bool run1time = true;

    void Start()
    {
        rt = gameObject.GetComponent<RectTransform>();
        root = rt.position;
        delta = destination.position - root;
    }

    public void Trigger_Move()
    {
        if (run1time)
        {
            run1time = false;
            StartCoroutine(move());
        }
    }

    private IEnumerator move()
    {
        float timer = 0f;
        float process;
        while (timer<=timeMove)
        {
            timer += Time.fixedDeltaTime;
            process = timer / timeMove;
            rt.position = root + (process * delta);
            yield return 0;
        }
    } 
}
