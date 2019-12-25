using System.Collections;
using System.Collections.Generic;
using Unity.Extentison;
using UnityEngine;
using UnityEditor;
using TMPro;
using UnityEngine.UI;

public class FixFont : EditorWindow
{
    [MenuItem("Window/TextMeshPro/Fix font")]
    public static void ShowWindow()
    {
        GetWindow<FixFont>("Fix font");
    }
    
    private TMP_FontAsset font;

    void OnGUI()
    {
        GUILayout.Label("Fix all font and text in current scene");
        GUILayout.Toggle(true, "Don't change size");
        font = (TMP_FontAsset)EditorGUILayout.ObjectField(font, typeof(TMP_FontAsset), true);
        if (GUILayout.Button("Fix now"))
            if (font != null)
            {
                int counter = 0;
                foreach (TextMeshProUGUI obj in SceneAsset.FindObjectsOfType<TextMeshProUGUI>())
                    if (obj.font != font)
                    {
                        counter++;
                        //obj.font = font;
                    }
                foreach (Text obj in SceneAsset.FindObjectsOfType<Text>())
                {
                    try
                    {
                        GameObject att = obj.gameObject;
                        RectTransform oldrt = att.GetComponent<RectTransform>();
                        RectTransform rt = oldrt.GetCopyOf(new RectTransform());    
                        DestroyImmediate(obj);
                        DestroyImmediate(att.GetComponent<CanvasRenderer>());
                        att.AddComponent<TextMeshProUGUI>();
                        att.GetComponent<TextMeshProUGUI>().font = font;
                        oldrt = rt.GetCopyOf(new RectTransform());
                    }
                    catch { }
                    counter++;

                }
                Debug.Log(counter + " item(s) had been fixed!");
            }
        
    }
}
