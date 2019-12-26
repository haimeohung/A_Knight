using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRotate : MonoBehaviour
{
    [SerializeField] private Vector3 rotate = Vector3.zero;
    void Update()
    {
        transform.Rotate(rotate * Time.deltaTime);
    }
}
