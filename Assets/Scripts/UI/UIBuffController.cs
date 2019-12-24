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
    private PlayerControler2D player;

    void Start()
    {
        slider = gameObject.GetComponent<Slider>();
        timer = item.timeEffect;
        player = FindObjectOfType<PlayerControler2D>();
        if (setOnBuffStart is null)
            setOnBuffStart = item.getEffectStart(player);
        if (setOnBuffEnd is null)
            setOnBuffEnd = () => {
                item.getEffectEnd(player)();
                Destroy(gameObject);
            };
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
