using System.Collections;
using System.Collections.Generic;
using Unity.Extentison;
using UnityEngine;

public class ZombieController : EntityController
{
    [Header("Other")]
    Animator ani;
    private class ZombieCollider : MonoBehaviour
    {
        public int a = 5;
    }

    new void Start()
    {
        base.Start();
        foreach (var sprite in gameObject.GetComponentsInChildren<SpriteRenderer>())
        {
            sprite.gameObject.AddComponent<ZombieCollider>();
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
