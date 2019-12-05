using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingRockMovement : MonoBehaviour
{
    [SerializeField] float circle = 3f;
    [SerializeField] [Range(0.01f, 1f)] float length = 0.25f;
    float timer = 0f;

    void Start()
    {
        StartCoroutine(DelayStart());
    }

    IEnumerator DelayStart()
    {
        timer = 0f;
        float wait = Random.Range(circle / 2, circle);
        while (timer<wait)
        {
            timer += Time.fixedDeltaTime;
            yield return 0;
        }
        timer = 0f;
        if (Random.value > 0.5f)
            StartCoroutine(MoveUp());
        else
            StartCoroutine(MoveDown());
    }

    IEnumerator MoveDown()
    {
        while (timer > 0)
        {
            timer -= Time.fixedDeltaTime;
            transform.Translate(Vector3.down * Time.fixedDeltaTime * length);
            yield return 0;
        }
        StartCoroutine(MoveUp());
    }

    IEnumerator MoveUp()
    {
        while (timer < circle)
        {
            timer += Time.fixedDeltaTime;
            transform.Translate(Vector3.up * Time.fixedDeltaTime * length);
            yield return 0;
        }
        StartCoroutine(MoveDown());
    }
}
