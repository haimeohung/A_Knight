using UnityEngine;

class FinalController : SceneController
{
    [SerializeField] private GameObject input;
    [SerializeField] private PlayerControler2D player;
    [SerializeField] private DialogSystem dialog;
    [SerializeField] private UIFadeTransition transition;
    [SerializeField] private EntityController thirdDialogTrigger;
    [SerializeField] private UISliderController BossHP;


    void Start()
    {
        DisableControl();
        thirdDialogTrigger.OnDie += () => { DisableControl(); dialog.StartDialog("Final1"); };
        transition.OnFadeOutDone += (e) => { dialog.StartDialog("Final"); };
        transition.OnFadeInDone += (e) => { ChangeScene("WorldMap", ""); };
        dialog.OnDialogEnd += () =>
        {
            input.SetActive(true);
            player.enabled = true;
            thirdDialogTrigger?.gameObject.SetActive(true);
        };
    }

    private void DisableControl()
    {
        player.enabled = false;
        input.SetActive(false);
        thirdDialogTrigger?.gameObject.SetActive(false);
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
            }
        }
    }
}

