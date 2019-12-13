using System;
using System.Collections;
using UnityEngine; 
namespace Unity.Extentison
{
    public static class MyExtensions
    {
        public static float Magnitude2D(this Vector3 vector) => Mathf.Sqrt((vector.x * vector.x) + (vector.y * vector.y));
        public static bool isNull(this Transform transform) => transform is null || transform.Equals(null);
        public static float signedAngle(this Vector3 vector) => vector == Vector3.zero ? 0f : Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;
        public static float signedAngle(this Vector2 vector) => vector == Vector2.zero ? 0f : Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;
        public static void SetParentWithoutChangeScale(this Transform c, Transform p = null, Vector3? pos = null)
        {
            if (p is null)
            {
                GameObject g = GameObject.Find("ContainerRoot");
                if (g is null) g = new GameObject("ContainerRoot");
                c.SetParent(g.transform);
            }
            else
            {
                Transform h = p.Find("Container");
                if (h is null)
                {
                    GameObject g = new GameObject("Container");
                    g.transform.SetParent(p);
                    g.transform.localPosition = Vector3.zero;
                    c.SetParent(g.transform);
                    h = g.transform;
                }
                c.SetParent(h);
            }
            if (pos.HasValue) c.localPosition = pos.Value;
        }
    }
}