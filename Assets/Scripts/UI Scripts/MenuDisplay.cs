using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuDisplay : MonoBehaviour
{
    [SerializeField] [Range(0.2f, 1f)] private float timeMove = 0.5f;
    [SerializeField] [Range(0f, 0.5f)] private float timeBetweenObjet = 0.25f;
    [SerializeField] private float length = 400f;
    private List<RectTransform> objs = new List<RectTransform>();
    private List<float> root = new List<float>();
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
    }

    public void Trigger_ShowMenu()
    {
        if (timer <= 0)
            StartCoroutine(ShowMenu());
    }

    public void Trigger_CloseMenu()
    {
        if (timer>=0)
            StartCoroutine(CloseMenu());
    }

    IEnumerator ShowMenu()
    {
        timer = 0f;
        int doneObj = 0;
        float process;
        float done = length + timeBetweenObjet * nObj;

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
    }

    IEnumerator CloseMenu()
    {
        timer = 0f;
        int doneObj = 0;
        float process;
        float done = length + timeBetweenObjet * nObj;
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
    }
}
