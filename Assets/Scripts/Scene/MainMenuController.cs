using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : SceneController
{
    [SerializeField] private UIFadeTransition transition;
    [SerializeField] private TipList tipList;

    void Awake()
    {
        tips = tipList;
    }

    void Start()
    {
        transition.OnFadeInDone += (e) =>
        {
            if (e.name == "Play") 
                ChangeScene("WorldMap","");
        };
    }
}
