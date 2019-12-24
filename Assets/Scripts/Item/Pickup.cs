using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    private UIInventory inventory;
    [SerializeField] private Item item;
    void Start()
    {
        inventory = FindObjectOfType<UIInventory>();
        gameObject.GetComponent<Animator>().runtimeAnimatorController = item.ani;
        StartCoroutine(AssignHitBox());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            inventory?.AddNewItem(item, 1);
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
