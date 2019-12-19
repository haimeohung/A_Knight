using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Extentison;

public class SpawnMachine : MonoBehaviour
{
    public enum SpawnMode { Auto, Trigger }
    private enum LimitedMode { StopSpawn, DeleteFirst }

    [SerializeField] private SpawnMode spawnMode = SpawnMode.Trigger;
    [SerializeField] private GameObject what;
    [SerializeField] private LimitedMode limitedMode = LimitedMode.DeleteFirst;
    [SerializeField] [Range(1, 10000)] int limit = 1;
    [SerializeField] private float delayTimeSpawn = 1f;
    [SerializeField] private float randomDelayDelta = 0f;
    [Header("Info")]
    [SerializeField] private int numberOfClone;

    private Queue<GameObject> items = new Queue<GameObject>();
    public System.Action<GameObject> SetOnInit { private get; set; } = null;
    public System.Action<GameObject> SetOnDelete { private get; set; } = (g) => { Destroy(g); };
    void Start()
    {
        what.SetActive(false);
        if (gameObject is null)
            Debug.Log("Not assign spawn game object");
        else
            if (spawnMode == SpawnMode.Auto)
                StartCoroutine(running());
    }
    IEnumerator running()
    {
        while (true)
        {
            if (items.Count >= limit)
            {
                switch (limitedMode)
                {
                    case LimitedMode.DeleteFirst:
                        SetOnDelete(items.Dequeue());
                        numberOfClone = items.Count;
                        break;
                    case LimitedMode.StopSpawn:
                        while (items.Count >= limit)
                            yield return 0;
                        break;
                }
            }
            GameObject gene = Instantiate(what);
            try { SetOnInit(gene); } catch { }
            gene.transform.SetParentWithoutChangeScale(null, transform.position);
            gene.SetActive(true);
            try { SetOnInit(gene); } catch { }
            items.Enqueue(gene);
            numberOfClone = items.Count;

            yield return new WaitForSeconds(delayTimeSpawn + Random.Range(0, randomDelayDelta));
        }
    }

    public void Trigger_Spawn(int number = 1)
    {
        if (spawnMode == SpawnMode.Auto)
            return;

        if (items.Count >= limit)
        {
            switch (limitedMode)
            {
                case LimitedMode.DeleteFirst:
                    SetOnDelete(items.Dequeue());
                    numberOfClone = items.Count;
                    break;
                case LimitedMode.StopSpawn:
                    if (items.Count >= limit)
                        return;
                    break;
            }
        }
        GameObject gene = Instantiate(what);
        gene.transform.SetParentWithoutChangeScale(null, transform.position);
        gene.SetActive(true);
        try { SetOnInit(gene); } catch { }
        items.Enqueue(gene);
        numberOfClone = items.Count;
    }
}
