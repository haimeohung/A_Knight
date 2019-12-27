using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "TipList", menuName = "ScriptableObject/TipList...")]
public class TipList : ScriptableObject
{
    public List<Tip> list;
}

public class TipListEditor : EditorWindow
{
    TipList tipList, oldTipList;
    string tipString = "";
    Sprite tipSprite = null;
    bool canRepair;

    [MenuItem("Window/TipList Editor %&t")]
    static void Init()
    {
        GetWindow(typeof(TipListEditor));
    }
    void OnEnable()
    {
        tipList = AssetDatabase.LoadAssetAtPath<TipList>("Assets/GameObject/TipList.asset");
    }

    void OnGUI()
    {
        canRepair = EditorGUILayout.Toggle("Repair", canRepair);
        if (canRepair)
        {
            oldTipList = EditorGUILayout.ObjectField("Chọn TipList hỏng", oldTipList, typeof(TipList), false) as TipList;
            if (GUILayout.Button("Repair..."))
            {
                var tmp = oldTipList.list;
                tipList = CreateInstance<TipList>();
                tipList.list = new List<Tip>();
                AssetDatabase.CreateAsset(tipList, "Assets/GameObject/TipList.asset");
                AssetDatabase.SaveAssets();

                foreach (var item in tmp)
                {
                    Tip tip = CreateInstance<Tip>();
                    tip.tip = item.tip;
                    tip.image = item.image;
                    tipList.list.Add(tip);
                    AssetDatabase.AddObjectToAsset(tip, tipList);
                    AssetDatabase.SaveAssets();
                }
                canRepair = false;
            }
        }
        EditorGUILayout.Space();
        if (GUILayout.Button("Create New TipList List"))
        {
            tipList = CreateInstance<TipList>();
            tipList.list = new List<Tip>();
            AssetDatabase.CreateAsset(tipList, "Assets/GameObject/TipList.asset");
            AssetDatabase.SaveAssets();
        }

        tipString = EditorGUILayout.TextField("Nhập hướng dẫn", tipString);
        tipSprite = EditorGUILayout.ObjectField("Chọn hình ảnh", tipSprite, typeof(Sprite), false) as Sprite;

        if (GUILayout.Button("Add New Tip"))
        {
            Tip tip = CreateInstance<Tip>();
            tip.tip = tipString;
            tip.image = tipSprite;
            tipList.list.Add(tip);
            AssetDatabase.AddObjectToAsset(tip, tipList);
            AssetDatabase.SaveAssets();
        }

        if (GUI.changed)
            EditorUtility.SetDirty(tipList);
    }
}