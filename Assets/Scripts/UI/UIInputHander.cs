using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.tag;

public class UIInputHander : MonoBehaviour
{
    public Vector3 atkDirection;

    private VJHander[] joysticks;
    private ButtonControler[] buttons;

    private AttackMode _attackMode = AttackMode.None;
    public AttackMode AttackMode
    {
        get => _attackMode;
        set
        {
            try
            {
                if (value != _attackMode)
                {
                    switch (value)
                    {
                        case AttackMode.None:
                            buttons[(int)ButtonTag.Attack]?.gameObject.SetActive(false);
                            joysticks[(int)JoystickTag.Weapon]?.gameObject.SetActive(false);
                            break;
                        case AttackMode.NonDirection:
                            buttons[(int)ButtonTag.Attack]?.gameObject.SetActive(true);
                            joysticks[(int)JoystickTag.Weapon]?.gameObject.SetActive(false);
                            break;
                        case AttackMode.HaveDirection:
                            buttons[(int)ButtonTag.Attack]?.gameObject.SetActive(false);
                            joysticks[(int)JoystickTag.Weapon]?.gameObject.SetActive(true);
                            break;
                    }
                    _attackMode = value;
                }
            }
            catch
            {
                StartCoroutine(FixSetAttackMode(value));
            }
        }
    }
    IEnumerator FixSetAttackMode(AttackMode mode)
    {
        yield return 0;
        AttackMode = mode;
    }

    public int movingDirection
    {
        get
        {
            if (IsPress(ButtonTag.Left) && IsPress(ButtonTag.Right))
                return 0;
            if (IsPress(ButtonTag.Left))
                return -1;
            if (IsPress(ButtonTag.Right))
                return 1;
            return 0;
        }
    }

    void Start()
    {
        joysticks = new VJHander[System.Enum.GetValues(typeof(JoystickTag)).Length];
        buttons = new ButtonControler[System.Enum.GetValues(typeof(ButtonTag)).Length];
        foreach (var item in GetComponentsInChildren<VJHander>())
            joysticks[(int)item.joystickTag] = item;
        foreach (var item in GetComponentsInChildren<ButtonControler>())
            buttons[(int)item.buttonTag] = item;
    }

    public bool IsPress(ButtonTag tag) => tag == ButtonTag.None || buttons[(int)tag] is null ? false : buttons[(int)tag].IsPress;
    public bool OnButtonUp(ButtonTag tag) => tag == ButtonTag.None || buttons[(int)tag] is null ? false : buttons[(int)tag].OnButtonUp;
    public bool OnButtonDown(ButtonTag tag) => tag == ButtonTag.None || buttons[(int)tag] is null ? false : buttons[(int)tag].OnButtonDown;
    public Vector3 GetDirection(JoystickTag tag) => tag == JoystickTag.None || buttons[(int)tag] is null ? Vector3.zero : joysticks[(int)tag].GetInputDirection;
}