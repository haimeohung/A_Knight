using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Extentison;

public class EntityPostCollision : MonoBehaviour
{
    public float timeFade = 2f;

    public float timer;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.layer == 12)
            return;
        //transform.SetParentWithoutChangeScale(collision.transform);
        foreach (var comp in gameObject.GetComponents<Component>())
        {
            if (!((comp is Transform) || (comp is SpriteRenderer) || (comp is EntityPostCollision)))
            {
                UnityEngine.Object.Destroy(comp);
            }
        }
        TriggerSpecialEffect();
    }

    public virtual void TriggerSpecialEffect() { }
}

