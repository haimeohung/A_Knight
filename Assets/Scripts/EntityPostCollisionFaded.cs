using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityPostCollisionFaded : EntityPostCollision
{
    public override void TriggerSpecialEffect()
    {
        StartCoroutine(fadeout());
    }

    IEnumerator fadeout()
    {
        timer = timeFade;
        SpriteRenderer sprite = gameObject.GetComponent<SpriteRenderer>();
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, timer / timeFade);
            yield return 0;
        }
        Destroy(gameObject);
    }
}
