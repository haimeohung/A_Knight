using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundList", menuName = "ScriptableObject/SoundList...")]
public class SoundList : ScriptableObject
{
    public List<Sound> list;
}

public class SoundListEditor : EditorWindow
{
    SoundList SoundList, oldSoundList;
    string sName = "";
    AudioClip sAudioClip = null;
    public float sVolume = 1f;
    public float sPitch = 1f;
    public bool sLoop = false;
    bool canRepair;

    [MenuItem("Window/SoundList Editor %&s")]
    static void Init()
    {
        GetWindow(typeof(SoundListEditor));
    }
    void OnEnable()
    {
        SoundList = AssetDatabase.LoadAssetAtPath<SoundList>("Assets/GameObject/SoundList.asset");
    }

    void OnGUI()
    {
        canRepair = EditorGUILayout.Toggle("Repair", canRepair);
        if (canRepair)
        {
            oldSoundList = EditorGUILayout.ObjectField("Chọn SoundList hỏng", oldSoundList, typeof(SoundList), false) as SoundList;
            if (GUILayout.Button("Repair..."))
            {
                var tmp = oldSoundList.list;
                SoundList = CreateInstance<SoundList>();
                SoundList.list = new List<Sound>();
                AssetDatabase.CreateAsset(SoundList, "Assets/GameObject/SoundList.asset");
                AssetDatabase.SaveAssets();

                foreach (var item in tmp)
                {
                    Sound s = CreateInstance<Sound>();
                    s.name = item.name;
                    s.clip = item.clip;
                    s.volume = item.volume;
                    s.pitch = item.pitch;
                    s.loop = item.loop;
                    SoundList.list.Add(s);
                    AssetDatabase.AddObjectToAsset(s, SoundList);
                    AssetDatabase.SaveAssets();
                }
                canRepair = false;
            }
        }
        EditorGUILayout.Space();
        if (GUILayout.Button("Create New SoundList List"))
        {
            SoundList = CreateInstance<SoundList>();
            SoundList.list = new List<Sound>();
            AssetDatabase.CreateAsset(SoundList, "Assets/GameObject/SoundList.asset");
            AssetDatabase.SaveAssets();
        }

        sName = EditorGUILayout.TextField("Tên sound", sName);
        sAudioClip = EditorGUILayout.ObjectField("Chọn AudioClip", sAudioClip, typeof(AudioClip), false) as AudioClip;
        sVolume = EditorGUILayout.Slider("Âm lượng", sVolume, 0f, 1f);
        sPitch = EditorGUILayout.Slider("Pitch", sPitch, 0.1f, 3f);
        sLoop = EditorGUILayout.Toggle("Tự động lặp", sLoop);

        if (GUILayout.Button("Add New Sound"))
        {
            Sound s = CreateInstance<Sound>();
            s.name = sName;
            s.clip = sAudioClip;
            s.volume = sVolume;
            s.pitch = sPitch;
            s.loop = sLoop;
            SoundList.list.Add(s);
            AssetDatabase.AddObjectToAsset(s, SoundList);
            AssetDatabase.SaveAssets();
        }

        if (GUI.changed)
            EditorUtility.SetDirty(SoundList);
    }
}