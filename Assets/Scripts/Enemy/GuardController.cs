using System.Collections;
using System.Collections.Generic;
using Unity.Extentison;
using UnityEngine;

public class GuardController : EntityController
{
    [Header("Other")]
    Animator ani;
    EntityInfo info;
    private class GuardCollider : MonoBehaviour
    {
        public int a = 5;
    }

    new void Start()
    {
        base.Start();
        info = gameObject.GetComponent<EntityInfo>();
        foreach (var sprite in gameObject.GetComponentsInChildren<SpriteRenderer>())
        {
            sprite.gameObject.AddComponent<GuardCollider>();
        }
    }
    new void Update()
    {
        base.Update();
       

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }
    public void Die()
    {
        gameObject.explode();
    }

    IEnumerator delayDie()
    {
        yield return new WaitForSeconds(1f);
        ani = gameObject.GetComponent<Animator>();
        ani.enabled = false;
        gameObject.explode();
        Destroy(gameObject);
    }
}
