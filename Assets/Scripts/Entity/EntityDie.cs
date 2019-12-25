using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Extentison;

public class EntityDie
{
    public void explode(GameObject gameObject)
    {
        Transform fx = gameObject.transform.Find("Explosion");
        fx?.gameObject.SetActive(true);
        fx?.SetParentWithoutChangeScale();
        fx?.gameObject.AddComponent<DeleteTimer>();
        fx?.gameObject.GetComponent<DeleteTimer>()?.setDelayTime(3f);
        var objs = gameObject.GetComponentsInChildren<SpriteRenderer>();
        for (int i = objs.Length-1; i >= 0; i--)
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
            rb.velocity = new Vector2(Random.Range(-10, 10), Random.Range(0, 10));
        }
    }
}
