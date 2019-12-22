using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class UIBuffsController : MonoBehaviour
{
    [SerializeField] private float space = 3f;
    [SerializeField] private GameObject buffPrefab;
    private int member = 0;

    void Update()
    {
        member = transform.childCount;
        CheckMember();
        CheckSpace();
    }

    #region member
    private int oldMember = -1;
    private void CheckMember()
    {
        if (member != oldMember)
        {
            oldMember = member;
            ReSize();
        }
    }
    #endregion
    #region space
    private float oldSpace;
    private void CheckSpace()
    {
        if (oldSpace != space)
        {
            oldSpace = space;
            ReSize();
        }
    }
    #endregion

    private void ReSize()
    {
        int i = 0;
        foreach (Transform t in transform)
        {
            RectTransform rt = t.GetComponent<RectTransform>();
            rt.localPosition = new Vector3((60 + space) * i, rt.localPosition.y, rt.localPosition.z);
            i++;
        }
    }

    public void AddNewBuff(Item buffFromItem)
    {
        GameObject newBuff = Instantiate(buffPrefab, transform);
        newBuff.GetComponent<UIBuffController>().item = buffFromItem;
        CheckMember();
    }
}
