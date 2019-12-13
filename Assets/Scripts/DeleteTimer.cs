using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteTimer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(delete());
    }
    public void setDelayTime(float time)
    {
        delayTime = time;
    }
    public float delayTime = 0f;
    private IEnumerator delete()
    {
        yield return new WaitForSeconds(delayTime);
        Destroy(gameObject);
    }
}
