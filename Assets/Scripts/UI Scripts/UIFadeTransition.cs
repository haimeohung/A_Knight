using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(RawImage))]
public class UIFadeTransition : MonoBehaviour
{
    [SerializeField] [Range(0.1f, 1f)] private float time = 0.25f;
    private RawImage image;
    private float timer = 0f;
    void Start()
    {
        image = gameObject.GetComponent<RawImage>();
        StartCoroutine(FadeOut());
    }

    public void Trigger_FadeIn()
    {
        if (timer <= 0f)
            StartCoroutine(FadeIn());
    }
    
    public void Trigger_FadeOut()
    {
        if (timer <= 0f)
            StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        image.enabled = true;
        image.color = Color.black;
        timer = time;
        float process;
        while (timer >= 0)
        {
            timer -= Time.fixedDeltaTime;
            process = timer / time;
            image.color = new Color(0, 0, 0, process);
            yield return 0;
        }
        image.enabled = false;
        timer = -0f;
    }

    IEnumerator FadeIn()
    {
        image.enabled = true;
        image.color = new Color(0, 0, 0, 0);
        timer = 0f;
        float process;
        while (timer <= time)
        {
            timer += Time.fixedDeltaTime;
            process = timer / time;
            image.color = new Color(0, 0, 0, process);
            yield return 0;
        }
        image.color = Color.black;
        timer = -0f;
    }
}
