using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIItemController : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TMPro.TextMeshProUGUI text;
    public Item item;
    public System.Action setOnClick { private get; set; } = () => { };

    void Start()
    {
        image.sprite = item.image;
        text.text = item.number.ToString();
    }

    public void Click()
    {
        setOnClick();
    }
}
