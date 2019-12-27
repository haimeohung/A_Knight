using System.Collections;
using System.Collections.Generic;
using Unity.Extentison;
using UnityEngine;

public class EntityController: MonoBehaviour
{
    public event System.Action OnDie;
    [Header("Drop")]
    [SerializeField] protected List<Item> listDrop;
    [SerializeField] private GameObject samleItemDrop;
    EntityInfo info;
    protected void Start()
    {
        info = gameObject.GetComponent<EntityInfo>();
    }
    protected void Update()
    {
        if (info.HP_index <= 0)
        {
            OnDie?.Invoke();
            gameObject.ChangeLayerCompletely(18);
            gameObject.explode();
            gameObject.slowFade(10);
            DropTriger();
            Destroy(this);
        }
    }
    protected void DropTriger(float exDropRate = 1f)
    {
        foreach (var item in listDrop)
            if (RandomPlus.getValue() < item.dropRate * exDropRate) 
            {
                GameObject g = Instantiate(samleItemDrop);

                g.transform.SetParentWithoutChangeScale(null, transform.position);
            }
    }
}
