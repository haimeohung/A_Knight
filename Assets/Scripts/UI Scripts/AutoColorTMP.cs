using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class AutoColorTMP : MonoBehaviour
{
    [SerializeField] private Color startColor, endColor;
    [SerializeField] [Range(2f, 8f)] private float cycle = 4f;
    [SerializeField] private float sleep = 1f;
    private TextMeshProUGUI target;
    private float privateTimer = 0f, halfCycle;
    private bool stop = false;
    void Start()
    {
        target = gameObject.GetComponent<TextMeshProUGUI>();
        privateTimer = 0f;
        halfCycle = cycle / 2;
        StartCoroutine(next());
    }

    private IEnumerator next()
    {
        while (privateTimer < halfCycle)
        {
            privateTimer += Time.fixedDeltaTime;
            target.color = startColor + (endColor - startColor) * (privateTimer / halfCycle);
            if (stop)
            {
                privateTimer = halfCycle - privateTimer;
                StartCoroutine(back());
                goto END;
            }
            yield return 0;
        }
        privateTimer -= halfCycle + sleep;
        StartCoroutine(wait());
    END: { }
    }

    private IEnumerator wait()
    {
        while (privateTimer < 0)
        {
            privateTimer += Time.fixedDeltaTime;
            if (stop)
                break;
            yield return 0;
        }
        StartCoroutine(back());
    }

    private IEnumerator back()
    {
        while (privateTimer < halfCycle)
        {
            privateTimer += Time.fixedDeltaTime;
            target.color = endColor + (startColor - endColor) * (privateTimer / halfCycle);
            yield return 0;
        }
        privateTimer -= halfCycle;
        if (!stop)
            StartCoroutine(next());
        else
            gameObject.SetActive(false);
    }

    public void Trigger_StopCircle()
    {
        stop = true;
    }
}
