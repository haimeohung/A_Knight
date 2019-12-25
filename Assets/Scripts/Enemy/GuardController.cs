using System.Collections;
using System.Collections.Generic;
using Unity.Extentison;
using UnityEngine;

public class GuardController : MonoBehaviour
{
    Animator ani;
    EntityInfo info;
    bool UpdateOneTime = true;
    private class GuardCollider : MonoBehaviour
    {
        public int a = 5;
    }
    void Start()
    {
        info = gameObject.GetComponent<EntityInfo>();
        foreach (var sprite in gameObject.GetComponentsInChildren<SpriteRenderer>())
        {
            sprite.gameObject.AddComponent<GuardCollider>();
            Debug.Log(sprite.gameObject.name);
        }
    }
    void Update()
    {

        if (info.HP_index <= 0 && UpdateOneTime)
        {          
            //StartCoroutine(delayDie());
            UpdateOneTime = false;
        }

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
