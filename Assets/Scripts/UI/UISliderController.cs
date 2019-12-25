using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//[ExecuteInEditMode]
public class UISliderController : MonoBehaviour
{
    [Header("Setting")]
    [SerializeField] private int maxValue = 100;
    public Color color = Color.white;
    [Header("Real-time status")]
    [SerializeField] private float _value = 0;
    [SerializeField] private float _autoRecoveryPerSecond = 0;
    [Header("Slow-down status")]
    [SerializeField] private float timeDelay = 1f;
    [SerializeField] [Range(1f, 100f)] private float autoDrop = 20f;
    private float _dropValue = 0f;

    [HideInInspector] public RawImage[] images;
    [HideInInspector] public TextMeshProUGUI text;
    [HideInInspector] public Slider[] sliders;
    private float dropHPtimer;

    public float value { get => _value; set => _value = value; }
    public float autoRecoveryPerSecond { get => _autoRecoveryPerSecond; set => _autoRecoveryPerSecond = value; }
    public int MaxValue { get => maxValue; set => maxValue = value; }

    #region check for update
    #region Value
    private float oldValue = float.PositiveInfinity;
    private void CheckValue()
    {
        if (oldValue != _value) 
        {
            float tmp = oldValue;
            oldValue = Mathf.Clamp(_value, 0f, maxValue);
            _value = oldValue;
            sliders[1].value = _value / maxValue;
            if (_value < tmp)
            {
                if (timer <= 0)
                    StartCoroutine(DropDow());
                else
                    timer = timeDelay;
            }
        }
    }
    #endregion
    #region Slow-down
    private void UpdateSlowDown()
    {
        if (_dropValue < _value)
            _dropValue = _value;
        else if (timer <= 0)
            _dropValue -= Time.fixedDeltaTime * autoDrop;
        
        sliders[0].value = _dropValue / maxValue;
    }
    private float timer = -0f;
    private IEnumerator DropDow()
    {
        timer = timeDelay;
        while (timer > 0)
        {
            timer -= Time.fixedDeltaTime;
            yield return 0;
        }
    }
    #endregion
    #endregion
    private void Start()
    {
        images = gameObject.GetComponentsInChildren<RawImage>();
        sliders = gameObject.GetComponentsInChildren<Slider>();
        text = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        
        CheckValue();
    }

    void Update()
    {
        _value += Time.deltaTime * _autoRecoveryPerSecond;
        CheckValue();
        UpdateSlowDown();
    }
}