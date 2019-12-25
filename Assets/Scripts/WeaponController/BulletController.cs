using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Extentison;

public class BulletController : MonoBehaviour
{
    Rigidbody2D rigi;
    bool stopUpdate = false;
    void Start()
    {
        rigi = gameObject.GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(rigi);
        Destroy(this);
    }
}
