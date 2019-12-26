
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(RawImage))]
public class UIFadeTransition : MonoBehaviour
{
    public event System.Action<Object> OnFadeInDone;
    public event System.Action<Object> OnFadeOutDone;
    [SerializeField] [Range(0.1f, 1f)] private float time = 0.25f;
    [SerializeField] private bool autoStart = true;
    private RawImage image;
    private float timer = 0f;

    private Object eventData;

    void Start()
    {
        image = gameObject.GetComponent<RawImage>();
        if (autoStart)
            StartCoroutine(FadeOut());
    }

    public void Trigger_FadeIn(Object eventData)
    {
        if (timer <= 0f)
        {
            this.eventData = eventData;
            StartCoroutine(FadeIn());
        }
    }
    
    public void Trigger_FadeOut(Object eventData)
    {
        if (timer <= 0f)
        {
            this.eventData = eventData;
            StartCoroutine(FadeOut());
        }
    }

    IEnumerator FadeOut()
    {
        image.enabled = true;
        image.color = Color.black;
        timer = time;
        float process;
        while (timer >= 0)
        {
            timer -= Time.deltaTime;
            process = timer / time;
            image.color = new Color(0, 0, 0, process);
            yield return 0;
        }
        image.enabled = false;
        timer = -0f;
        OnFadeOutDone?.Invoke(eventData);
    }

    IEnumerator FadeIn()
    {
        image.enabled = true;
        image.color = new Color(0, 0, 0, 0);
        timer = 0f;
        float process;
        while (timer <= time)
        {
            timer += Time.deltaTime;
            process = timer / time;
            image.color = new Color(0, 0, 0, process);
            yield return 0;
        }
        image.color = Color.black;
        timer = -0f;
        OnFadeInDone?.Invoke(eventData);
    }
}
