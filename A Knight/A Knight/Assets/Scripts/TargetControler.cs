using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetControler : MonoBehaviour
{
    int _selectedWeapon = 0;
    public int SelectedWeapon
    {
        get => _selectedWeapon;
        set
        {
            weapons[_selectedWeapon].parent.SetActive(false);
            _selectedWeapon = value;
            weapons[_selectedWeapon].parent.SetActive(true);
        }
    }

    [SerializeField] private List<Nodes> weapons = new List<Nodes>();
    [SerializeField] private Nodes target;

    public bool IsScalar { get; set; } = false;

    [System.Serializable] class Nodes
    {
        public Transform left, right;
        public GameObject parent;
        public Nodes() { }
        public Nodes(GameObject weapon)
        {
            this.parent = weapon;
            parent.SetActive(false);
            left = parent.transform.GetChild(0);
            right = parent.transform.GetChild(1);
        }
    }

    void Start()
    {
        foreach (Transform child in transform)
            weapons.Add(new Nodes(child.gameObject));
        weapons[_selectedWeapon].parent.SetActive(true);

        SelectedWeapon = 0;
    }

    void Update()
    {
        try
        {
            weapons[SelectedWeapon].parent.SetActive(true);
            target.left.position = weapons[SelectedWeapon].left.position;
            target.right.position = weapons[SelectedWeapon].right.position;
        }
        catch
        {
            Debug.LogError("Null object");
        }
    }
}
