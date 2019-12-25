using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Unity.Extentison;

[CustomEditor(typeof(UISliderController))]
public class UISliderBarEditer : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        try
        {
            UISliderController target = (UISliderController)this.target;

            target.images[0].color = target.color.MultiplyAlpha(0.5f);
            target.images[1].color = target.color;
            target.images[2].color = Color.white.MultiplyAlpha(target.color.a);

            if (target.autoRecoveryPerSecond >= 0)
                target.text.text = "+" + target.autoRecoveryPerSecond + "/s";
            else
                target.text.text = "–" + (-target.autoRecoveryPerSecond) + "/s";

            target.sliders[0].value = target.value / target.MaxValue;
            target.sliders[1].value = target.value / target.MaxValue;
        }
        catch { }
    }
}
