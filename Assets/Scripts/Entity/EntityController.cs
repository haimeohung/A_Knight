using System.Collections;
using System.Collections.Generic;
using Unity.Extentison;
using UnityEngine;

public class EntityController : MonoBehaviour
{
    public event System.Action OnDie;
    [Header("Drop")]
    [SerializeField] [Range(1f, 100f)] private float exDropRate = 1;
    [SerializeField] protected List<Item> listDrop;
    [SerializeField] private GameObject samleItemDrop;
    private PlayerControler2D player;
    public EntityInfo info;
    protected void Start()
    {
        info = gameObject.GetComponent<EntityInfo>();
        player = gameObject.GetComponent<PlayerControler2D>();
    }
    protected void Update()
    {
        if (info.HP_index <= 0)
        {
            SoundManager.instance.Play("zombie_death");
            OnDie?.Invoke();
            gameObject.layer = 18;
            gameObject.explode();
            DropTriger(exDropRate);
            gameObject.slowFade(10);
            try
            {
                Destroy(gameObject.GetComponent<Animator>());
            }
            catch { }
            Destroy(this);
            Debug.Log("die");
            return;
        }
    }
    protected void DropTriger(float exDropRate = 1f)
    {
        foreach (var item in listDrop)
            if (RandomPlus.getValue() < item.dropRate * exDropRate)
            {
                GameObject g = Instantiate(samleItemDrop);
                g.SetActive(true);
                g.GetComponent<LoadItem>()?.Load(item);
                g.transform.SetParentWithoutChangeScale(null, transform.position);
            }
    }
    
}
