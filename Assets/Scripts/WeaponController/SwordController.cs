using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Extentison;

public class SwordController : MonoBehaviour
{
    private SpawnMachine fx;
    private float time = 0.2f;
    [SerializeField] private PlayerControler2D player;
    void Start()
    {
        fx = gameObject.GetComponent<SpawnMachine>();
        fx.SetOnInit += (o) => { o.slowFade(0.5f); };
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (time <= 0)
        {
            time = 0.2f;
        }
        else
        {
            time -= Time.deltaTime;
            return;
        }
        if (collision.gameObject.layer == 12)
        {
            Debug.Log(Time.frameCount);
            EntityInfo info = collision.gameObject.GetFirstComponentInParent<EntityInfo>();
            info?.BeAttacked(player.atk + 4);
            if (info?.HP_index > player?.atk + 4) SoundManager.instance.Play("player_injured");

            fx?.Trigger_Spawn();
        }
 
    }

}
