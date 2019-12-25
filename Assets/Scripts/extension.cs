using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Cryptography;
using UnityEngine;

namespace Unity.Extentison
{
    public static class MyExtensions
    {
        public static float Magnitude2D(this Vector3 vector) => Mathf.Sqrt((vector.x * vector.x) + (vector.y * vector.y));
        public static bool isNull(this Transform transform) => transform is null || transform.Equals(null);
        public static float signedAngle(this Vector3 vector) => vector == Vector3.zero ? 0f : Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;
        public static float signedAngle(this Vector2 vector) => vector == Vector2.zero ? 0f : Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;
        public static Color MultiplyAlpha(this Color color, float alpha) =>  new Color(color.r, color.g, color.b, color.a * alpha);
        public static Color ChangeAlpha(this Color color, float alpha) => new Color(color.r, color.g, color.b, alpha);
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
        public static T GetCopyOf<T>(this Component comp, T other) where T : Component
        {
            Type type = comp.GetType();
            if (type != other.GetType()) return null; // type mis-match
            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default | BindingFlags.DeclaredOnly;
            PropertyInfo[] pinfos = type.GetProperties(flags);
            foreach (var pinfo in pinfos)
            {
                if (pinfo.CanWrite)
                {
                    try
                    {
                        pinfo.SetValue(comp, pinfo.GetValue(other, null), null);
                    }
                    catch { } 
                }
            }
            FieldInfo[] finfos = type.GetFields(flags);
            foreach (var finfo in finfos)
            {
                finfo.SetValue(comp, finfo.GetValue(other));
            }
            return comp as T;
        }
        public static T RandomList<T>(this IList<T> list) => list.Count == 0 ? default : list[RandomPlus.getRange(0, list.Count)];
    }

    public abstract class RandomPlus
    {
        private static double get()
        {
            RandomNumberGenerator i = RandomNumberGenerator.Create();
            byte[] data = new byte[8];
            i.GetBytes(data);
            long t = 0;
            foreach (var item in data)
                t = (t << 8) | item;
            t &= long.MaxValue;
            return (double)t / long.MaxValue;
        }
        /// <summary>
        /// Return random number between 0 (include) and 1 (exclude)
        /// </summary>
        public static float getValue() => (float)get();
        /// <summary>
        /// Return random number between lower (include) and upper (exclude)
        /// </summary>
        public static float getRange(float lower, float upper) => (float)get() * (upper - lower) + lower;
        /// <summary>
        /// Return random number between lower (include) and upper (exclude)
        /// </summary>
        public static int getRange(int lower, int upper) => (int)Math.Floor(get() * (upper - lower) + lower);
    }

    public static class EntityDie
    {
        public static void explode(this GameObject gameObject)
        {
            Transform fx = gameObject.transform.Find("Explosion");
            fx?.gameObject.SetActive(true);
            fx?.SetParentWithoutChangeScale();
            fx?.gameObject.AddComponent<DeleteTimer>();
            fx?.gameObject.GetComponent<DeleteTimer>()?.setDelayTime(3f);
            var objs = gameObject.GetComponentsInChildren<SpriteRenderer>();
            for (int i = objs.Length - 1; i >= 0; i--)
            {
                objs[i].transform.SetParentWithoutChangeScale();
                foreach (var comp in objs[i].gameObject.GetComponents<Component>())
                {
                    if (!((comp is Transform) || (comp is SpriteRenderer) || (comp is Collider2D)))
                    {
                        UnityEngine.Object.Destroy(comp);
                    }
                }

                objs[i].gameObject.AddComponent<Rigidbody2D>();
                objs[i].gameObject.AddComponent<EntityPostCollisionFaded>();
                Rigidbody2D rb = objs[i].gameObject.GetComponent<Rigidbody2D>();
                System.Random r = new System.Random();
                rb.velocity = new Vector2(r.Next(-10, 10), r.Next(0, 10));
            }
        }
        private class SlowAutoFade : MonoBehaviour
        {
            private class AutoFade : MonoBehaviour
            {
                SpriteRenderer[] sprites;
                public float time { get; set; } = 2f;
                [SerializeField] private float timer;
                void Start()
                {
                    sprites = gameObject.GetComponentsInChildren<SpriteRenderer>();
                    timer = time;
                }

                void Update()
                {
                    float percent = timer / time;
                    foreach (SpriteRenderer sprite in sprites)
                        sprite.color = new Color(sprite.color.r, sprite.color.b, sprite.color.b, percent);
                    timer -= Time.deltaTime;
                    if (percent <= 0)
                        Destroy(gameObject);
                }
            }
            public void Trigger(float delay)
            {
                StartCoroutine(trigger(delay));
            }
            IEnumerator trigger(float delay)
            {
                yield return new WaitForSeconds(delay);
                gameObject.AddComponent<AutoFade>();
                Destroy(this);
            }
        }
        public static void slowFade(this GameObject gameObject, float delay = 0f)
        {
            gameObject.AddComponent<SlowAutoFade>();
            gameObject.GetComponent<SlowAutoFade>().Trigger(delay);
        }
    }
}