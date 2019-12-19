using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Extentison;

[RequireComponent(typeof(Rigidbody2D))]
public class ArrowController : MonoBehaviour
{
    Rigidbody2D rigi;
    bool stopUpdate = false;
    void Start()
    {
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
        transform.SetParentWithoutChangeScale(collision.transform);

        Destroy(this);
        Destroy(rigi);
    }
}
