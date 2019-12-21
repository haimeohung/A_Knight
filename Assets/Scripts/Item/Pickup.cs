using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    private Inventory inventory;
    public GameObject itemButton;
    void Start()
    {
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            for (int i = 0; i < inventory.items.Length; i++)
            {
                if (inventory.IsFull[i] == false)
                {
                    inventory.IsFull[i] = true;
                    Instantiate(itemButton, inventory.items[i].transform, false);
                    Destroy(gameObject);
                    break;
                }
            }
        }
    }
    void Update()
    {
    }
}
