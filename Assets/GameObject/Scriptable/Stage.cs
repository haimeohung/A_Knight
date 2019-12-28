using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

    
[CreateAssetMenu(fileName = "Stage", menuName = "ScriptableObject/Stage...")]
public class Stage : ScriptableObject
{
    public string nameStageShow;
#if UNITY_EDITOR
    public SceneAsset scene;
#endif
    public string sceneName;
    public string description;
}
