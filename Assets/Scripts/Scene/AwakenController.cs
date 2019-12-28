using UnityEngine;

class AwakenController : SceneController
{
    [SerializeField] private GameObject input;
    [SerializeField] private PlayerControler2D player;
    [SerializeField] private DialogSystem dialog;
    [SerializeField] private UIFadeTransition transition;
    [SerializeField] private GameObject secondDialogTrigger;
    [SerializeField] private EntityController thirdDialogTrigger;
    [SerializeField] private UISliderController BossHP;
    [SerializeField] private GateController gate;
    [SerializeField] private SaveGame save;
    private int numberDialog = 0;

    void Start()
    {
        SoundManager.instance.Stop("theme");
        SoundManager.instance.Play("night");

        DisableControl();
        secondDialogTrigger.AddComponent<Trigger>();
        secondDialogTrigger.GetComponent<Trigger>().trigger = () => { DisableControl(); dialog.StartDialog("Awaken1"); };
        thirdDialogTrigger.OnDie += () => { BossHP.gameObject.SetActive(false); DisableControl(); dialog.StartDialog("Awaken2"); gate.canTrigger = true; };
        transition.OnFadeInDone += (e) => 
        {
            if (e.Equals(null))
            {
                save.isPlayed = true;
                save.StageUnlock = 2;
            }
            ChangeScene("WorldMap", "");
        };
        transition.OnFadeOutDone += (e) => { dialog.StartDialog("Awaken");

        };
        dialog.OnDialogEnd += () =>
        {
            numberDialog++;
            if (numberDialog == 2)
                BossHP.gameObject.SetActive(true);
            input.SetActive(true);
            player.enabled = true;
        };
    }

    private void DisableControl()
    {
        input.SetActive(false);
        player.enabled = false;
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

