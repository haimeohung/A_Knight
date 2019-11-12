using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxDraw : MonoBehaviour
{
    [SerializeField] private bool debugMode = true;
    Vector2 size;
    Vector3 offset;

    void Start()
    {
        BoxCollider2D box = GetComponent<BoxCollider2D>();
        size = new Vector2(box.size.x * transform.localScale.x, box.size.y * transform.localScale.y);
        offset = new Vector3(box.offset.x * transform.localScale.x, box.offset.y * transform.localScale.y, 0);
    }

    void OnDrawGizmos()
    {
        if (!debugMode)
            return;

        Gizmos.color = Color.red;
        float wHalf = (size.x * .5f);
        float hHalf = (size.y * .5f);
        Vector3 topLeftCorner = new Vector3(transform.position.x - wHalf, transform.position.y + hHalf, 1f);
        Vector3 topRightCorner = new Vector3(transform.position.x + wHalf, transform.position.y + hHalf, 1f);
        Vector3 bottomLeftCorner = new Vector3(transform.position.x - wHalf, transform.position.y - hHalf, 1f);
        Vector3 bottomRightCorner = new Vector3(transform.position.x + wHalf, transform.position.y - hHalf, 1f);
        topLeftCorner += offset;
        topRightCorner += offset;
        bottomLeftCorner += offset;
        bottomRightCorner += offset;
        Gizmos.DrawLine(topLeftCorner, topRightCorner);
        Gizmos.DrawLine(topRightCorner, bottomRightCorner);
        Gizmos.DrawLine(bottomRightCorner, bottomLeftCorner);
        Gizmos.DrawLine(bottomLeftCorner, topLeftCorner);
    }
}
