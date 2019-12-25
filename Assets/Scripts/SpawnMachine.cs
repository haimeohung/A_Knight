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

    private List<GameObject> items = new List<GameObject>();
    public event System.Action<GameObject> SetOnInit;
    public event System.Action<GameObject> SetOnDelete;
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
                        SetOnDelete(items[0]);
                        items.RemoveAt(0);
                        numberOfClone = items.Count;
                        break;
                    case LimitedMode.StopSpawn:
                        while (items.Count >= limit)
                        {
                            for (int i = 0; i < items.Count; i++)
                                if (items[i] == null)
                                    items.RemoveAt(i);
                            yield return 0;
                        }
                        break;
                }
            }
            GameObject gene = Instantiate(what);
            gene.transform.SetParentWithoutChangeScale(null, what.transform.position);
            gene.SetActive(true);
            try { SetOnInit(gene); } catch { }
            items.Add(gene);
            numberOfClone = items.Count;
            yield return new WaitForSeconds(delayTimeSpawn + Random.Range(0, randomDelayDelta));
        }
    }

    public void Trigger_Spawn()
    {
        if (spawnMode == SpawnMode.Auto)
            return;

        if (items.Count >= limit)
        {
            switch (limitedMode)
            {
                case LimitedMode.DeleteFirst:
                    SetOnDelete(items[0]);
                    items.RemoveAt(0);
                    numberOfClone = items.Count;
                    break;
                case LimitedMode.StopSpawn:
                    while (items.Count >= limit)
                    {
                        for (int i = 0; i < items.Count; i++)
                            if (items[i] == null)
                                items.RemoveAt(i);
                        return;
                    }
                    break;
            }
        }
        GameObject gene = Instantiate(what);
        gene.transform.SetParentWithoutChangeScale(null, what.transform.position);
        gene.SetActive(true);
        try { SetOnInit(gene); } catch { }
        items.Add(gene);
        numberOfClone = items.Count;
    }
}
