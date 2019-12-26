using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Stage", menuName = "ScriptableObject/Stage...")]
public class Stage : ScriptableObject
{
    public string nameStageShow;
    public SceneAsset scene;
    public string description;
}
