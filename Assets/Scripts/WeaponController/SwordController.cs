using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Extentison;

public class SwordController : MonoBehaviour
{
    private SpawnMachine fx;

    void Start()
    {
        fx = gameObject.GetComponent<SpawnMachine>();
        fx.SetOnInit += (o) => { o.slowFade(0.5f); };

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 12)
        {
            fx?.Trigger_Spawn();
        }
    }
}
