using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Extentison;

public class BulletController : MonoBehaviour
{
    Rigidbody2D rigi;
    bool stopUpdate = false;
    [SerializeField] private PlayerControler2D player;
    void Start()
    {
        player.MP.value -= 5;
        rigi = gameObject.GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 12)
        {
            EntityInfo info = collision.gameObject.GetFirstComponentInParent<EntityInfo>();
            info?.BeAttacked(player.atk + 4);
        }
        transform.SetParentWithoutChangeScale(collision.gameObject.transform);
        Destroy(rigi);
        Destroy(this);
    }
}
