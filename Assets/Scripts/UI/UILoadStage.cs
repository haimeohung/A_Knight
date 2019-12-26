using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UILoadStage : MonoBehaviour
{
    public Stage stage;
    [SerializeField] private TextMeshProUGUI name, description;
    [SerializeField] private Animator ani;
    [SerializeField] private Image graphic;
    private Canvas canvas;
    public event System.Action<UILoadStage> setOnStateChange;
    private bool _state = false;
    public bool state
    {
        get => _state;
        set
        {
            if (_state != value)
            {
                _state = value;
                setOnStateChange.Invoke(this);
            }
        }
    }
    void Awake()
    {
        setOnStateChange += (e) => 
        {
            ani.SetBool("IsShow", state);
            if (state)
                canvas.sortingOrder = 1;
            else
                canvas.sortingOrder = 2;
        };
    }

    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(() => { state = !state; });
        canvas = gameObject.GetComponentInChildren<Canvas>();
        if (stage is null)
            return;
        name.text = stage.nameStageShow;
        description.text = stage.description;
    }

    public void ChangeState(bool? value = null)
    {
        if (value is null)
            state = !state;
        else
            state = value.Value;
    }

    public void SetImage(Sprite lockStageSprite)
    {
        graphic.sprite = lockStageSprite;
    }

    public void ChangeScene()
    {
        if (stage is null)
            return;
        UIFadeTransition transition = FindObjectOfType<UIFadeTransition>();
        transition.Trigger_FadeIn(null);
        transition.OnFadeInDone += (e) => { SceneController.ChangeScene(stage.scene.name, stage.nameStageShow); };
    }
}
