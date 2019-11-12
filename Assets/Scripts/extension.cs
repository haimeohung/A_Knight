using System;
using UnityEngine;

namespace Unity.Extentison
{
    public static class MyExtensions
    {
        public static float Magnitude2D(this Vector3 vector) => Mathf.Sqrt((vector.x * vector.x) + (vector.y * vector.y));
        public static bool isNull(this Transform transform) => transform is null || transform.Equals(null);
    }
}