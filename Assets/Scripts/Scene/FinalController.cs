using UnityEngine;

class FinalController : SceneController
{
    [SerializeField] private GameObject input;
    [SerializeField] private PlayerControler2D player;
    [SerializeField] private DialogSystem dialog;
    [SerializeField] private UIFadeTransition transition;
    [SerializeField] private EntityController thirdDialogTrigger;
    [SerializeField] private UISliderController BossHP;
    int dialogCounter = 0;

    void Start()
    {
        SoundManager.instance.Play("night");

        DisableControl();
        thirdDialogTrigger.OnDie += () => { DisableControl(); dialog.StartDialog("Final2"); };
        transition.OnFadeOutDone += (e) => { dialog.StartDialog("Final"); };
        transition.OnFadeInDone += (e) => { ChangeScene("WorldMap", ""); };
        dialog.OnDialogEnd += () =>
        {
            input.SetActive(true);
            player.enabled = true;
            if (thirdDialogTrigger != null) thirdDialogTrigger.gameObject.SetActive(true);
            if (dialogCounter == 1)
                if (BossHP != null) BossHP.gameObject.SetActive(true);
        };
    }

    private void DisableControl()
    {
        player.enabled = false;
        input.SetActive(false);
        if (dialogCounter == 0)
            thirdDialogTrigger?.gameObject.SetActive(false);
        dialogCounter++;
    }

    private class Trigger : MonoBehaviour
    {
        public System.Action trigger;
        private bool update1time = true;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer == 8 && update1time)
            {
                update1time = false;
                trigger();
                Destroy(gameObject);
                SoundManager.instance.Play("wind");

            }
        }
    }
}

