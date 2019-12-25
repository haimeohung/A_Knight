using System.Collections;
using System.Collections.Generic;
using Unity.Extentison;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadingSceneController : SceneController
{
    [Header("Slow-Down")]
    [SerializeField] [Range(0f, 10f)] private float minTimeLoad = 1f;
    [Header("Display")]
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI nameDisplay, tip;
    [SerializeField] private Slider slider;
    [SerializeField] private Button button;

    private AsyncOperation async;

    void Start()
    {
        Tip tip = tips.list.RandomList();
        image.sprite = tip.image;
        image.SetNativeSize();
        this.tip.text = tip.tip;
        nameDisplay.text = _nextSceneName;
        button.gameObject.SetActive(false);

        StartCoroutine(Load());
    }

    IEnumerator Load()
    {
        yield return new WaitForSeconds(0.5f);
        async = ChangeScene(_sceneName);
        async.allowSceneActivation = false;

        float timer = 0f;
        float minProcess = 0f;
        while (timer < minTimeLoad) 
        {
            minProcess = Mathf.Clamp(timer / minTimeLoad * 0.9f * RandomPlus.getValue(), minProcess, 1f);
            timer += Time.deltaTime;
            float step = (Time.deltaTime / minTimeLoad * 0.9f) * RandomPlus.getValue();
            minProcess += step;
            slider.value = minProcess;
            yield return 0;
        }

        while (!async.isDone) 
        {
            if (async.progress >= 0.9f)
            {
                button.gameObject.SetActive(true);
                slider.value = 1;
                break;
            }
            else
                slider.value = Mathf.Clamp(async.progress, minProcess, 1f);
            yield return 0;
        }
    }

    public void Changed()
    {
        async.allowSceneActivation = true;
    }
}
