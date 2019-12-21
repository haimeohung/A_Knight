using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[ExecuteInEditMode]
public class UILvControler : MonoBehaviour
{
    [SerializeField] private int _Lv = 1;
    [SerializeField] private long _exp = 0;
    [SerializeField] private long startRequestExp = 100;
    [SerializeField] [Range(0.0005f, 0.01f)] private float scaleExp = 0.0065f;
    private TextMeshProUGUI text;
    private Slider slider;
    void Start()
    {
        text = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        slider = gameObject.GetComponentInChildren<Slider>();
        CheckScaleExp();
        CheckLv();
        CheckExp();
    }

    void Update()
    {
        CheckScaleExp();
        CheckLv();
        CheckExp();
    }

    private long getExpRequest(int Lv) => (long)(Mathf.Pow(1 + scaleExp * Lv, Lv) * startRequestExp);

    #region check for update
    #region Lv
    private int oldLv = int.MinValue;
    [SerializeField] private long nextLvRequest;
    private void CheckLv()
    {
        if (oldLv != _Lv)
        {
            oldLv = _Lv;
            text.text = "Lv. " + _Lv;
            nextLvRequest = getExpRequest(_Lv);
        }
    }
    #endregion
    #region exp
    private long oldExp = long.MinValue;
    private void CheckExp()
    {
        if (oldExp != _exp)
        {
            oldExp = _exp;
            if (_exp >= nextLvRequest)
            {
                _Lv++;
                _exp -= nextLvRequest;
                CheckLv();
            }
            slider.value = (float)_exp / nextLvRequest;
        }
    }
    #endregion
    #region Scale Exp
    private float oldScaleExp = 0;
    private void CheckScaleExp()
    {
        if (oldScaleExp != scaleExp) 
        {
            oldScaleExp = scaleExp;
            nextLvRequest = getExpRequest(_Lv); 

        }
    }
    #endregion
    #endregion
}
