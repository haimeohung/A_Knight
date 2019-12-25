using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TrailRenderer))]
[ExecuteInEditMode]
public class FixTrailFX : MonoBehaviour
{
   
    void Update()
    {
        try
        {
            transform.rotation = transform.parent.localRotation;
        }
        catch { }
    }
}
