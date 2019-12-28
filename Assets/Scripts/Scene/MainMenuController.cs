using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainMenuController : SceneController
{
    [SerializeField] private UIFadeTransition transition;
    [SerializeField] private TipList tipList;
    [SerializeField] private SaveGame saveGame;
    [SerializeField] private TextMeshProUGUI playText;

    void Awake()
    {
        tips = tipList;
    }

    void Start()
    {
        SoundManager.instance.Play("theme");
        transition.OnFadeInDone += (e) =>
        {
            if (e.name == "Play") 
                ChangeScene("WorldMap","");
        };
        if (saveGame.isPlayed)
            playText.text = "TIẾP TỤC";
    }
}
