using UnityEngine;

class WarningController : MonoBehaviour
{
    [SerializeField] private UISliderController attachTo;
    [SerializeField] private UIFadeTransition transition;
    private Animator ani;

    void Start()
    {
        ani = gameObject.GetComponent<Animator>();
        attachTo.setOnValueDrop += () => 
        {
            ani?.SetTrigger("Lost");
            if (attachTo.value <= 0) 
            {
                Destroy(attachTo);
            }
        };
    }

    void Update()
    {
        ani?.SetFloat("%", attachTo.value / attachTo.MaxValue);
    }

    public void ReloadScene()
    {
        var ret = SceneController.ReLoadScene();
        if (transition != null)
            ret.completed += (e) => { transition.Trigger_FadeOut(null); };
    }
}