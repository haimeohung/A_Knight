using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadItem : MonoBehaviour
{
    public void Load(Item item)
    {
        gameObject.GetComponent<Animator>().runtimeAnimatorController = item.ani;
    }
}
