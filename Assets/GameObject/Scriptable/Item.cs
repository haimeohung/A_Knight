using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObject/Item...")]
public class Item : ScriptableObject
{
    public RuntimeAnimatorController ani;
    public new string name;
    public Sprite buffDisplay;
    public Sprite image;
    public float timeEffect;
    public string locationDrop;
    public float dropRate;
    public string info;
    public Effect effect;
    public string effectDescription;
    public enum Effect
    {
        low_hp,
        high_hp,
        low_mp,
        high_mp,
        low_sp,
        high_sp,
        low_atk,
        high_atk,
        low_def,
        high_def,
        low_speed,
        high_speed,
    }

    
    private System.Action setOnBuffStart;
    private System.Action setOnBuffEnd;


    public System.Action getEffectStart(PlayerControler2D player)
    {
        switch (effect)
        {
            case Effect.high_hp:
                return () => player.HP.autoRecoveryPerSecond += 10;
            case Effect.low_hp:
                return () => player.HP.autoRecoveryPerSecond += 5;
            case Effect.high_mp:
                return () => player.MP.autoRecoveryPerSecond += 10;
            case Effect.low_mp:
                return () => player.MP.autoRecoveryPerSecond += 5;
            case Effect.high_sp:
                return () => player.SP.autoRecoveryPerSecond += 10;
            case Effect.low_sp:
                return () => player.SP.autoRecoveryPerSecond += 5;
            case Effect.high_atk:
                return () => player.atk += 20;
            case Effect.low_atk:
                return () => player.atk += 5;
            case Effect.high_def:
                return () => player.atk += 20;
            case Effect.low_def:
                return () => player.def += 5;
            case Effect.high_speed:
                return () => player.runSpeed += 3;
            case Effect.low_speed:
                return () => player.runSpeed += 6;
            default:
                return () => { };
        };
    }
    public System.Action getEffectEnd(PlayerControler2D player)
    {
        switch (effect)
        {
            case Effect.high_hp:
                return () => player.HP.autoRecoveryPerSecond -= 10;
            case Effect.low_hp:
                return () => player.HP.autoRecoveryPerSecond -= 5;
            case Effect.high_mp:
                return () => player.MP.autoRecoveryPerSecond -= 10;
            case Effect.low_mp:
                return () => player.MP.autoRecoveryPerSecond -= 5;
            case Effect.high_sp:
                return () => player.SP.autoRecoveryPerSecond -= 10;
            case Effect.low_sp:
                return () => player.SP.autoRecoveryPerSecond -= 5;
            case Effect.high_atk:
                return () => player.atk -= 20;
            case Effect.low_atk:
                return () => player.atk -= 5;
            case Effect.high_def:
                return () => player.def -= 20;
            case Effect.low_def:
                return () => player.def -= 5;
            case Effect.high_speed:
                return () => player.runSpeed -= 6;
            case Effect.low_speed:
                return () => player.runSpeed -= 3;
            default:
                return () => { };
        }

    }
}

