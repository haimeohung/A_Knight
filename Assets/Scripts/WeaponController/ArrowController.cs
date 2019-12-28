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
        SoundManager.instance.Play("arrow");
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
        try
        {
            if (collision.gameObject.layer == 8)
                return;
            if (collision.gameObject.layer == 12)
            {
                EntityInfo info = collision.gameObject.GetFirstComponentInParent<EntityInfo>();
                info?.BeAttacked(player.atk + 3);
                if (info.HP_index > player.atk + 3) SoundManager.instance.Play("player_injured");
            }
            transform.SetParentWithoutChangeScale(collision.transform);
            Destroy(this);
            Destroy(rigi);
        }
        catch { }
    }
}
