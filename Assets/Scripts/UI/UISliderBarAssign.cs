using UnityEngine;

[RequireComponent(typeof(UISliderController))]
class UISliderBarAssign : MonoBehaviour
{
    [SerializeField] EntityInfo attachTo;
    private UISliderController slider;

    void Start()
    {
        slider = GetComponent<UISliderController>();
        slider.MaxValue = attachTo.HP_index;
        slider.autoRecoveryPerSecond = 0;
    }

    void Update()
    {
        slider.value = attachTo.HP_index;
    }
}