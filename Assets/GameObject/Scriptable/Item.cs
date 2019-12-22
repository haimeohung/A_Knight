using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Item",menuName ="Item/Item...")]
public class Item : ScriptableObject
{
    public new string name;
    public int number;
    public Sprite buffDisplay;
    public Sprite image;
    public float timeEffect;
    public string locationDrop;
    public string effectDescription;
    public string info;
    public System.Action setOnEffect = () => { };
}
