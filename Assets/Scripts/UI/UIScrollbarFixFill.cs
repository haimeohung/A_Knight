using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
[RequireComponent(typeof(RawImage))]
public class UIScrollbarFixFill : MonoBehaviour
{
    [SerializeField] private RectTransform hander;
    private Scrollbar scrollbar;
    private RectTransform rt;
    private float delta;

    private Color _color;
    public Color color
    {
        get => _color;
        set
        {
            _color = value;
            hander.gameObject.GetComponent<RawImage>().color = _color;
            gameObject.GetComponent<RawImage>().color = _color;
        }
    }
    void Start()
    {
        rt = gameObject.GetComponent<RectTransform>();
        scrollbar = gameObject.GetComponentInParent<Scrollbar>();
        delta = rt.rect.width;
    }

    void Update()
    {
        rt.offsetMax = new Vector2(-(1 - scrollbar.value) * delta - hander.rect.width + 2, rt.offsetMax.y);
    }
}
