using System.Collections;
using System.Collections.Generic;
using Unity.tag;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonControler : MonoBehaviour
{
    public ButtonTag buttonTag = ButtonTag.None;
    [SerializeField] private InputMode inputMode = InputMode.Touch;
    [SerializeField] private bool enableEffect = true;
    [Range(0.1f, 1f)] [SerializeField] private float timeEffect = 0.5f;
    [SerializeField] private Vector3 endSize = new Vector3(2f, 2f, 1f);
    [Range(0f,1f)] [SerializeField] private float deltaTouchSize = 0.6f;

    private RectTransform target, self;
    private Image image;
    private Image backGround;
    private float step, radius;
    private int isRunning = 0;
    private KeyCode keyCode;

    private bool _IsPress = false;
    public bool IsPress
    {
        get => _IsPress;
        private set
        {
            if (enableEffect)
                if (value)
                {
                    if (!_IsPress)
                        EffectTrigger();
                    try
                    {
                        backGround.enabled = true;
                    }
                    catch { };

                    OnButtomUp = false;
                    OnButtonDown = !_IsPress;
                }
                else
                {
                    try
                    {
                        backGround.enabled = false;
                    }
                    catch { };

                    OnButtomUp = _IsPress;
                    OnButtonDown = false;
                }
            _IsPress = value;
        }
    }
    public bool OnButtomUp { get; private set; }
    public bool OnButtonDown { get; private set; }

    void Start()
    {
        self = gameObject.GetComponent<RectTransform>();
        radius = (self.sizeDelta.x + self.sizeDelta.y) / 4 * deltaTouchSize;
        var tmp = gameObject.transform.GetChild(0);
        target = tmp?.gameObject.GetComponent<RectTransform>();
        image = tmp?.gameObject.GetComponent<Image>();
        backGround = gameObject.transform.GetChild(1)?.GetComponent<Image>();
        try
        {
            backGround.enabled = false;
        }
        catch { };
        if (!(target is null))
            target.localScale = new Vector3(0f, 0f, 1f);

        switch (buttonTag)
        {
            case ButtonTag.Jump:
                keyCode = KeyCode.Space;
                break;
            case ButtonTag.Left:
                keyCode = KeyCode.LeftArrow;
                break;
            case ButtonTag.Right:
                keyCode = KeyCode.RightArrow;
                break;
        }
    }

    void Update()
    {
        if (inputMode == InputMode.Touch)
        {
            if (Input.touchCount > 0)
                CheckPress();
            else
            {
                IsPress = false;
                OnButtomUp = false;
                OnButtonDown = false;
            }
        }
        else
        {
            IsPress = Input.GetKey(keyCode);
        }
    }

    private void CheckPress()
    {
        foreach (var item in Input.touches)
            if ((int)item.phase <= 2 && Vector2.Distance(item.position, self.position) <= radius)
            {
                IsPress = true;
                return;
            }
        IsPress = false;
    }

    public void EffectTrigger()
    {
        if (target is null || image is null)
            return;
        isRunning++;
        if (isRunning > 1)  
            return;
        StartCoroutine(RunningTrigger());
    }
    IEnumerator RunningTrigger()
    {
        float timer = 0f;
        float process;
        Color color = image.color;
        Color change = image.color;
        while (timer < timeEffect) 
        {
            if (isRunning > 1)
            {
                timer = 0f;
                isRunning = 1;
            }
            timer += Time.fixedDeltaTime;
            process = timer / timeEffect;
            target.localScale = endSize * process;
            change.a = 1f-process;
            image.color = change;
            yield return null;
        }
        target.localScale = new Vector3(0f, 0f, 1f);
        image.color = color;
        isRunning = 0;
    }
}
