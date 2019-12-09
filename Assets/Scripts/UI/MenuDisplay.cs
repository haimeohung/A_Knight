using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuDisplay : MonoBehaviour
{
    [SerializeField] [Range(0.2f, 1f)] private float timeMove = 0.5f;
    [SerializeField] [Range(0f, 0.5f)] private float timeBetweenObjet = 0.25f;
    [SerializeField] private float length = 400f;
    private List<RectTransform> objs = new List<RectTransform>();
    private List<float> root = new List<float>();
    private Button[] buttons;
    private int nObj;
    private float timer = 0f;

    void Start()
    {
        RectTransform rt = gameObject.GetComponent<RectTransform>();        
        nObj = rt.childCount;
        for (int i = 0; i < nObj; i++)
        {
            objs.Add(rt.GetChild(i).GetComponent<RectTransform>());
            root.Add(objs[i].localPosition.x);
        }
        buttons = GetComponentsInChildren<Button>();
        foreach (var item in buttons)
            item.enabled = false;
    }

    public void Trigger_ShowMenu()
    {
        if (timer <= 0)
        {
            StartCoroutine(ShowMenu());
            foreach (var item in buttons)
                item.enabled = false;
        }
    }

    public void Trigger_CloseMenu()
    {
        if (timer >= 0)
        {
            StartCoroutine(CloseMenu());
            foreach (var item in buttons)
                item.enabled = false;
        }
    }

    IEnumerator ShowMenu()
    {
        timer = 0f;
        int doneObj = 0;
        float process;
        float done = timeMove + timeBetweenObjet * (nObj - 1);

        while (timer < done)
        {
            timer += Time.fixedDeltaTime;
            for (int i = doneObj; i < nObj; i++)
                if (timer > timeBetweenObjet * i)
                {
                    process = (timer - timeBetweenObjet * i) / timeMove;
                    if (process >= 1) 
                    {
                        doneObj++;
                        process = 1;
                    }
                    objs[i].localPosition = new Vector3(root[i] - (process * length), objs[i].localPosition.y, objs[i].localPosition.z);
                }
            yield return 0;
        }
        foreach (var item in buttons)
            item.enabled = true;
    }

    IEnumerator CloseMenu()
    {
        timer = 0f;
        int doneObj = 0;
        float process;
        float done = timeMove + timeBetweenObjet * (nObj - 1);
        List<RectTransform> objs = this.objs;
        objs.Reverse();

        while (timer < done)
        {
            timer += Time.fixedDeltaTime;
            for (int i = doneObj; i < nObj; i++)
                if (timer > timeBetweenObjet * i)
                {
                    process = (timer - timeBetweenObjet * i) / timeMove;
                    if (process >= 1)
                    {
                        doneObj++;
                        process = 1;
                    }
                    objs[i].localPosition = new Vector3(root[i] - length + (process * length), objs[i].localPosition.y, objs[i].localPosition.z);
                }
            yield return 0;
        }
        timer = -0f;

        foreach (var item in buttons)
            item.enabled = true;
    }
}
