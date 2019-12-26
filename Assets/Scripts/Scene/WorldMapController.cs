using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldMapController : SceneController
{
    [SerializeField] private SaveGame save;
    [SerializeField] private Sprite lockStageSprite;
    private List<UILoadStage> loads = new List<UILoadStage>();
    private UILoadStage lastSelectd = null;
    void Start()
    {
        loads.AddRange(FindObjectsOfType<UILoadStage>());

        for (int i = 0; i < save.StageUnlock; i++)
        {
            if (i >= loads.Count)
                break;
            loads[i].setOnStateChange += (e) =>
            {
                if (e.state && !e.Equals(lastSelectd))
                {
                    lastSelectd?.ChangeState(false);
                    lastSelectd = e;
                }
            };

        }

        for (int i = save.StageUnlock; i < loads.Count; i++)
        {
            loads[i].GetComponent<Button>().enabled = false;
            loads[i].SetImage(lockStageSprite);
            Destroy(loads[i].GetComponentInChildren<AutoRotate>());
        }
    }
}
