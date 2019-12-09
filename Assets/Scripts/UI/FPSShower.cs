using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class FPSShower : MonoBehaviour
{
    private TextMeshProUGUI text;
    void Start()
    {
        text = gameObject.GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        try
        {
            text.text = ((int)(1f / Time.unscaledDeltaTime)).ToString();
        }
        catch
        { }
    }
}
