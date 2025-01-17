﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public UIInventory inventory;
    public Item item;
    public void Start()
    {
        gameObject.GetComponent<Animator>().runtimeAnimatorController = item.ani;
        if (inventory is null)
            inventory = FindObjectOfType<UIInventory>();
        StartCoroutine(AssignHitBox());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            inventory.AddNewItem(item, 1);
            Destroy(gameObject);
        }
    }

    IEnumerator AssignHitBox()
    {
        yield return 0;
        gameObject.AddComponent<PolygonCollider2D>();
        gameObject.GetComponent<PolygonCollider2D>().isTrigger = true;
    }
}
