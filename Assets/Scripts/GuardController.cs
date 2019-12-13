using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardController : MonoBehaviour
{
    EntityDie die = new EntityDie();
    Animator ani;
    void Start()
    {


        StartCoroutine(delayDie());
    }



    public void Die()
    {
        die.explode(gameObject);
    }

    IEnumerator delayDie()
    {
        yield return new WaitForSeconds(1f);
        ani = gameObject.GetComponent<Animator>();
        ani.enabled = false;
        die.explode(gameObject);
        Destroy(gameObject);
    }
}
