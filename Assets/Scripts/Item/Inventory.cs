using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public bool[] IsFull;
    public GameObject[] items;
    public int[] numbers;
    
    private void Start()
    {
        for (int i = 0; i < IsFull.Length; i++)
        {
            IsFull[i] = false;
        }
    }
}
