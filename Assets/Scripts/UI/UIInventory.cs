using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInventory : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI info, drop;
    [SerializeField] private Image buffIcon;
    [SerializeField] private GameObject sampleItem;
    [SerializeField] private Item initItem;
    [SerializeField] private Transform items;
    private Item _seletedItem = null;

    Hashtable hash = new Hashtable();

    public Item SelectedItem
    {
        get => _seletedItem;
        set
        {
            _seletedItem = value;
            info.text = value.info;
            if (value.locationDrop.Length > 0)
                drop.text = "Vị trí rơi: " + value.locationDrop + "\n";
            if (value.effectDescription.Length > 0)
                drop.text += "Hiệu quả: " + value.effectDescription;
            if (value.locationDrop.Length + value.effectDescription.Length == 0)
                drop.text = "";

            buffIcon.sprite = value.buffDisplay;
        }
    }
    public void Start()
    {
        SelectedItem = initItem;
    }

    public void AddNewItem(Item item)
    {
        if (hash.ContainsKey(item.name))
            (hash[item.name] as Item).number += item.number;
        else
        {
            hash.Add(item.name, item);
            foreach (Transform t in items)
                if (t.childCount == 0)
                {
                    GameObject newItem = Instantiate(sampleItem, t);
                    newItem.GetComponent<UIItemController>().item = item;
                    return;
                }
        }
    }
}
