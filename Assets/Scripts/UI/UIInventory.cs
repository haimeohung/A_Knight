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
    [SerializeField] private UIBuffsController buffs;
    [SerializeField] private PlayerControler2D player;
    private Item _seletedItem = null;

    private Hashtable hash = new Hashtable();
    private RectTransform rt;

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
        rt = gameObject.GetComponent<RectTransform>();
        if (buffs is null)
            buffs = FindObjectOfType<UIBuffsController>();
        if (player is null)
            player = FindObjectOfType<PlayerControler2D>();
        StartCoroutine(LateSetActice(false));
    }

    public void AddNewItem(Item item, int number = 1)
    {
        if (hash.ContainsKey(item.name))
            (hash[item.name] as Node).number += number;
        else
        {
            foreach (Transform t in items)
                if (t.childCount == 0)
                {
                    GameObject newItem = Instantiate(sampleItem, t);
                    UIDragDrop drag = newItem.GetComponent<UIDragDrop>();
                    drag.dragOut = rt;
                    drag.setOnDragEnd = (e) =>
                    {
                        foreach (Transform t2 in items)
                            if (drag.IsInsideRect(t2.GetComponent<RectTransform>(),e.position))
                            {
                                if (t2.childCount > 0)
                                    t2.GetChild(0).SetParent(t);
                                newItem.transform.SetParent(t2);
                                newItem.transform.localPosition = Vector3.zero;
                                return;
                            }
                    };
                    drag.setOnDragOut = () => {
                        Destroy(drag.gameObject);
                        hash.Remove(item.name);
                        SelectedItem = initItem;
                    };
                    UIItemController controller = newItem.GetComponent<UIItemController>();
                    controller.item = item;
                    controller.setOnClick = () => {
                        SelectedItem = item;
                    };
                    hash.Add(item.name, new Node(newItem) { setOnNumberChange = (n) => {
                        if (n == 0)
                        {
                            SelectedItem = initItem;
                            Destroy(drag.gameObject);
                            hash.Remove(item.name);
                        }
                        controller.SetNumber(n);
                    }, number = number });
                    return;
                }
        }
    }

    public void UseItem()
    {
        buffs.AddNewBuff(SelectedItem);
        (hash[SelectedItem.name] as Node).number--;
        player.ani.SetTrigger("Eat");
    }

    private class Node
    {
        public System.Action<int> setOnNumberChange { private get; set; } = (int n) => { };
        public int number
        {
            get => _number;
            set
            {
                if (value != _number) 
                {
                    _number = value;
                    setOnNumberChange(value);
                }
            }
        }

        private GameObject item;
        private int _number = -1;

        public Node(GameObject item = null)
        {
            this.item = item;
        }
    }

    IEnumerator LateSetActice(bool value)
    {
        yield return 0;
        transform.parent.gameObject.SetActive(value);
    }
}
