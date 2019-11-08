using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuDisplay : MonoBehaviour
{
    public bool moveImmediately;
    public float distance;
    public float timeMove;
    public float timeBetweenObjet;
    public float delayStart;
    
    private float dt;
    private Vector3 trans;
    private int nObj;
    private int doneObj;
    private float gotoPositionX;
    // Start is called before the first frame update
    void Start()
    {
        trans = new Vector3(timeMove == 0 ? float.PositiveInfinity : distance / timeMove, 0, 0);
        nObj = transform.childCount;
    }

    // Update is called once per frame
    void Update()
    {
        dt += Time.fixedDeltaTime;
        if (moveImmediately && dt >= 0) 
        {
            for (int i = doneObj; i < nObj; i++)
                if (dt > timeBetweenObjet * i)
                {
                    Transform obj = transform.GetChild(i);
                    if (Mathf.Abs(obj.localPosition.x - gotoPositionX) < trans.magnitude * Time.fixedDeltaTime)   
                    {
                        obj.localPosition = new Vector3(gotoPositionX, obj.localPosition.y, obj.localPosition.z);
                        doneObj++;
                        continue;
                    }
                    obj.localPosition += trans * Time.fixedDeltaTime;
                }
            if (doneObj == nObj)
                moveImmediately = false;
        }
    }

    public void StartMove(int direction)
    {
        dt = -delayStart;
        moveImmediately = true;
        doneObj = 0;
        gotoPositionX = transform.childCount > 0 ? transform.GetChild(0).localPosition.x : transform.localPosition.x;
        gotoPositionX += direction * distance;
        trans = new Vector3(Mathf.Abs(trans.x) * direction, 0, 0);
    }
}
