using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.tag;

public class UIInputHander : MonoBehaviour
{
    public Vector3 atkDirection;
    public int movingDirection;

    private VJHander[] joysticks;
    private ButtonControler[] buttons;
    private Dictionary<JoystickTag, int> joystickTagMap = new Dictionary<JoystickTag, int>();
    private Dictionary<ButtonTag, int> buttonTagMap = new Dictionary<ButtonTag, int>();

    // Start is called before the first frame update
    void Start()
    {
        joysticks = gameObject.GetComponentsInChildren<VJHander>();
        buttons = gameObject.GetComponentsInChildren<ButtonControler>();
        for (int i = 0; i < joysticks.Length; i++)
            if (joysticks[i].joystickTag != JoystickTag.None)
                joystickTagMap.Add(joysticks[i].joystickTag, i);
        for (int i = 0; i < buttons.Length; i++)
            if (buttons[i].buttonTag != ButtonTag.None)
                buttonTagMap.Add(buttons[i].buttonTag, i);
    }

    void Update()
    {
        movingDirection = 0;
        if (IsPress(ButtonTag.Left))
            movingDirection = -1;
        if (IsPress(ButtonTag.Right))
            movingDirection = 1;
        if (IsPress(ButtonTag.Left) && IsPress(ButtonTag.Right))
            movingDirection = 0;
    }

    public bool IsPress(ButtonTag tag) => tag == ButtonTag.None ? false : buttons[buttonTagMap[tag]].IsPress;
    public bool OnButtonUp(ButtonTag tag) => tag == ButtonTag.None ? false : buttons[buttonTagMap[tag]].OnButtomUp;
    public bool OnButtonDown(ButtonTag tag) => tag == ButtonTag.None ? false : buttons[buttonTagMap[tag]].OnButtonDown;
    public Vector3 GetDirection(JoystickTag tag) => tag == JoystickTag.None ? Vector3.zero : joysticks[joystickTagMap[tag]].GetInputDirection;
    public float GetAngle(JoystickTag tag) => tag == JoystickTag.None ? 0f : joysticks[joystickTagMap[tag]].GetInputAngle;
}
