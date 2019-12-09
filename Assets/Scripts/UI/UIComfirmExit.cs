using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class UIComfirmExit : MonoBehaviour
{
    [SerializeField] [Range(0.2f, 1f)] private float speed = 0.5f;
    private RectTransform rt;
    private bool IsZoom { get; set; } = false;
    void Start()
    {
        rt = gameObject.GetComponent<RectTransform>();
        rt.localScale = Vector3.zero;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !IsZoom)
            StartCoroutine(ZoomIn());
    }

    public void ZoomInTrigger()
    {
        if (!IsZoom)
            StartCoroutine(ZoomIn());
    }

    public void ZoomOutTrigger()
    {
        if (IsZoom)
            StartCoroutine(ZoomOut());
    }

    public void ExitTrigger()
    {
        try
        {
            //UnityEditor.EditorApplication.isPlaying = false;
        }
        catch { }
        Application.Quit();
    }

    IEnumerator ZoomIn()
    {
        IsZoom = true;
        float timer = 0f;
        float process;
        while (timer<speed)
        {
            timer += Time.deltaTime;
            process = timer / speed;
            rt.localScale = new Vector3(process, process, process);
            yield return 0;
        }
        rt.localScale = Vector3.one;
    }

    IEnumerator ZoomOut()
    {
        IsZoom = false;
        float timer = speed;
        float process;
        while (timer >=0)
        {
            timer -= Time.deltaTime;
            process = timer / speed;
            rt.localScale = new Vector3(process, process, process);
            yield return 0;
        }
        rt.localScale = Vector3.zero;
    }
}
