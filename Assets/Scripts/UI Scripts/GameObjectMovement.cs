using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectMovement : MonoBehaviour
{
    public Vector3 destination;
    public bool moveImmediately;
    public float timeMove;
    public float delayStart;

    private Vector3 trans;
    private float dt;
    // Start is called before the first frame update
    void Start()
    {
        trans = destination - transform.localPosition;
        trans = timeMove == 0f ? Vector3.positiveInfinity : trans / timeMove;
    }

    // Update is called once per frame
    void Update()
    {
        dt += Time.fixedDeltaTime;
        if (moveImmediately && dt >= 0) 
        {
            Vector3 v = trans * Time.fixedDeltaTime;
            if ((destination - transform.localPosition).magnitude <= v.magnitude) 
            {
                transform.localPosition = destination;
                moveImmediately = false;
                return;
            }
            transform.localPosition += v;
        }
    }

    public void StartMove()
    {
        moveImmediately = true;
        dt = -delayStart;
    }
}
