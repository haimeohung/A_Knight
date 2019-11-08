using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.tag;

public class VJHander : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    public JoystickTag joystickTag = JoystickTag.None;
    [SerializeField] private Vector3 inputDirection;
    [Range(0.0f, 1.0f)] [SerializeField] private float ActiveInput = 0.5f;


    private Image joystickContainer;
    private Image joystick;
    public Vector3 GetFixInputDirection
    {
        get
        {
            Vector3 ret = new Vector3(inputDirection.x, inputDirection.y, inputDirection.z);
            if (Mathf.Abs(inputDirection.x) > ActiveInput)
                ret.x = ret.x > 0 ? 1.0f : -1.0f;
            else
                ret.x = 0f;
            if (Mathf.Abs(inputDirection.y) > ActiveInput)
                ret.y = ret.y > 0 ? 1.0f : -1.0f;
            else
                ret.y = 0f;
            return ret;
        }
    }
    public Vector3 GetInputDirection { get => inputDirection; }
    public float GetInputAngle { get => inputDirection == Vector3.zero ? 0f : Mathf.Atan2(inputDirection.y, inputDirection.x) * 180f / Mathf.PI; }
    
    void Start()
    {
        joystickContainer = GetComponent<Image>();
        joystick = transform.GetChild(0).GetComponent<Image>();
        inputDirection = Vector3.zero;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 position = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(joystickContainer.rectTransform, eventData.position, eventData.pressEventCamera, out position);
        position.x /= joystickContainer.rectTransform.sizeDelta.x;
        position.y /= joystickContainer.rectTransform.sizeDelta.y;

        inputDirection = new Vector3(position.x * 2, position.y * 2, 0);
        inputDirection = (inputDirection.magnitude > 1) ? inputDirection.normalized : inputDirection;

        joystick.rectTransform.anchoredPosition = new Vector3(inputDirection.x * (joystickContainer.rectTransform.sizeDelta.x / 2), inputDirection.y * (joystickContainer.rectTransform.sizeDelta.y / 2));
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        inputDirection = Vector3.zero;
        joystick.rectTransform.anchoredPosition = Vector3.zero;
    }
}