using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class UIBuffController : MonoBehaviour
{
    public Item item;
    private Slider slider;
    private float timer = 0f;
    public System.Action setOnBuffStart { private get; set; } = null;
    public System.Action setOnBuffEnd { private get; set; } = null;
    void Start()
    {
        slider = gameObject.GetComponent<Slider>();
        timer = item.timeEffect;
        if (setOnBuffStart is null)
            setOnBuffStart = item.setOnEffect;
        if (setOnBuffEnd is null)
            setOnBuffEnd = () => { Destroy(gameObject); };
        gameObject.GetComponentInChildren<Image>().sprite = item.buffDisplay;
        setOnBuffStart();
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            setOnBuffEnd();
        }
        slider.value = 1 - (timer / item.timeEffect);
    }
}
