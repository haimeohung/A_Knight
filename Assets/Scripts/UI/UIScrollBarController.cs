using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIScrollBarController : MonoBehaviour
{
    [Header("Real-time status")]
    [SerializeField] private float _value = 0;
    [SerializeField] private float _autoRecoveryPerSecond = 0;
    [Header("Slow-down status")]
    [SerializeField] private float timeDelay = 1f;
    [SerializeField] private float autoDropHP = -20f;
    [SerializeField] private float autoDropHPMaxValue = 100f;
    [SerializeField] private float dropHP = 0f;
    [Header("Other setting")]
    [SerializeField] private int maxValue = 100;
    [SerializeField] private Color color;
    private Scrollbar[] scrollbar;
    private TextMeshProUGUI text;
    private float dropHPtimer;
    private bool _UpdateOneTime = true;
    public float AutoRecoveryPerSecond
    {
        get => _autoRecoveryPerSecond;
        set
        {
            _autoRecoveryPerSecond = value;
            try
            {
                if (value > 0)
                    text.text = "+" + value + "/s";
                else
                    text.text = value + "/s";
            }
            catch
            {
                if (_UpdateOneTime)
                    StartCoroutine(WaitSetText(value));
                _UpdateOneTime = false;
            }
        }
    }
    public float value
    {
        get => _value;
        set
        {
            float tmp = Mathf.Clamp(value, 0f, maxValue);
            if (tmp < _value) 
            {
                dropHPtimer = timeDelay;
                if (dropHP < tmp)
                    dropHP = tmp;
            }
            _value = tmp;
        }
    }

    void Start()
    {
        try
        {
            UIScrollbarFixFill[] colors = gameObject.GetComponentsInChildren<UIScrollbarFixFill>();
            colors[0].color = color * 0.6f;
            colors[1].color = color;
        }
        catch
        {
            StartCoroutine(WaitSetColor());
        }
        value = _value;
        scrollbar = gameObject.GetComponentsInChildren<Scrollbar>();
        text = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        AutoRecoveryPerSecond = _autoRecoveryPerSecond;

        dropHP = 0;
        scrollbar[0].value = 0;
    }
    
    void Update()
    {
        value += Time.deltaTime * AutoRecoveryPerSecond;
        scrollbar[1].value = value / maxValue;
        if (dropHP > value)
        {
            scrollbar[0].value = dropHP / autoDropHPMaxValue;
            if (dropHPtimer > 0)
                dropHPtimer -= Time.deltaTime;
            else
            {
                dropHP += Time.deltaTime * autoDropHP;
            }
        }
        else
            dropHP = value;
    }

    public void recudeHP(float value)
    {
        this.value -= value;
    }

    private IEnumerator WaitSetText(float value)
    {
        yield return 0;
        AutoRecoveryPerSecond = value;
    }
    private IEnumerator WaitSetColor()
    {
        yield return 0;
        UIScrollbarFixFill[] colors = gameObject.GetComponentsInChildren<UIScrollbarFixFill>();
        colors[0].color = color * 0.6f;
        colors[1].color = color;
    }
}
