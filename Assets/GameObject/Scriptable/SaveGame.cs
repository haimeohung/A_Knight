using System.Collections;
using System.Collections.Generic;

using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "SaveGame", menuName = "ScriptableObject/SaveGame...")]
public class SaveGame : ScriptableObject
{
    public bool isPlayed = false;
    public int StageUnlock;
}
