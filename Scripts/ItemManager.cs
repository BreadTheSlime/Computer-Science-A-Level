using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public GameObject[] itemTable;
    public float[] dropChanceTable;
    public float total;
    public float randomNumber;
    [SerializeField] GameObject door;

    private void Start()
    {
        door = itemTable[9];
    }

    public GameObject TileDrop()
    {
        foreach (var item in dropChanceTable)
        {
            total += item;
        }

        randomNumber = Random.Range(0, total);

        total = 0;

        for (int i = 0; i <= itemTable.Length; i++)
        {
            if (randomNumber <= dropChanceTable[i])
            {
                if (itemTable[i] == door)
                {
                    dropChanceTable[i] = 0;
                }
                return itemTable[i];
            }
            else
            {
                randomNumber -= dropChanceTable[i];
            }
        }

        return null;
    }
}