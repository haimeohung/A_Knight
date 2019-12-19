using System.Collections;
using System.Collections.Generic;
using Unity.Extentison;
using UnityEngine;

public class GuardController : MonoBehaviour
{
    Animator ani;
    void Start()
    {
        StartCoroutine(delayDie());
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
