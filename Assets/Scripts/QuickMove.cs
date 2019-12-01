using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class QuickMove : MonoBehaviour
{
    Rigidbody2D rb;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    void Update()
    {
        rb.velocity = (new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"))) * 2;
    }
}
