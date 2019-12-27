using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Extentison;

[RequireComponent(typeof(Rigidbody2D))]
public class ArrowController : MonoBehaviour
{
    Rigidbody2D rigi;
    bool stopUpdate = false;
    [SerializeField] private PlayerControler2D player;
    void Start()
    {
        player.MP.value -= 5;
        rigi = gameObject.GetComponent<Rigidbody2D>();
    }

    void LateUpdate()
    {
        if (!stopUpdate)
            if (rigi.velocity.magnitude > 0.1)
                transform.eulerAngles = new Vector3(0, 0, rigi.velocity.signedAngle());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8)
            return;
        if (collision.gameObject.layer == 12)
        {
            EntityInfo info = collision.gameObject.GetFirstComponentInParent<EntityInfo>();
            info?.BeAttacked(player.atk + 4);
        }
        transform.SetParentWithoutChangeScale(collision.transform);
        Destroy(this);
        Destroy(rigi);
    }
}
