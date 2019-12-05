using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Extentison;

[ExecuteInEditMode]
public class LogicJoint : MonoBehaviour
{
    [SerializeField] private Transform target; // targer aim to
    private enum AutoConfigureType { none, horizontal, vertical, dependingObject }
    [SerializeField] private AutoConfigureType autoConfigure = AutoConfigureType.none;
    [SerializeField] private bool flipAngle = false;
    [SerializeField] private GameObject whatIsDependingObject;
    private Transform child;
    private float r1, r2, r1delta, r2delta;
    public void FlipAngle() { flipAngle = !flipAngle; }

    private bool? autoAngle;

    void Start()
    {
        child = transform.GetChild(0);
        Transform endTranform = child.GetChild(0);

        r1 = (child.position - transform.position).Magnitude2D();
        r2 = (endTranform.position - child.position).Magnitude2D();
        r1delta = -CaculateAngle(child.localPosition);
        r2delta = -CaculateAngle(endTranform.localPosition);
    }

    void Update()
    {
        if (target is null)
            return;

        Vector3 distance = target.position - transform.position;
        if (distance.Magnitude2D() < 0.000001) // too close
            return;
        if (distance.Magnitude2D() > (r1 + r2)) // too far
        {
            float angle = CaculateAngle(distance);
            transform.rotation = Quaternion.Euler(0, 0, angle + r1delta);
            child.rotation = Quaternion.Euler(0, 0, angle + r2delta);
            return;
        }
        // making triangle
        float angle1 = CaculateAngle(distance.magnitude, r1, r2);
        float angle2 = CaculateAngle(r1, r2, distance.magnitude);

        switch (autoConfigure)
        {
            case AutoConfigureType.vertical:
                autoAngle = target.position.x > transform.position.x;
                break;
            case AutoConfigureType.horizontal:
                autoAngle = target.position.y > transform.position.y;
                break;
            case AutoConfigureType.dependingObject:
                autoAngle = whatIsDependingObject?.transform.eulerAngles.y > 90f;
                break;
            default:
                autoAngle = false;
                break;
        }

        bool canFlip = flipAngle;
        if (autoAngle.HasValue)
            canFlip = flipAngle ^ autoAngle.Value;

        if (canFlip)
        {
            angle1 = -angle1;
            angle2 = -angle2;
        }
        angle1 += CaculateAngle(distance);
        angle2 += angle1 - 180;

        transform.rotation = Quaternion.Euler(0, 0, angle1 + r1delta);
        child.rotation = Quaternion.Euler(0, 0, angle2 + r2delta);
    }

    float CaculateAngle(Vector3 vector) => Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;
    float CaculateAngle(float a, float b, float c) => Mathf.Acos((a * a + b * b - c * c) / (2 * a * b)) * Mathf.Rad2Deg; // angle between edge a and b
}