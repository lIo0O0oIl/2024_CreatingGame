using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private int maxStackItem = 64;
    [SerializeField] private InventorySlot[] inventorySlots;
    [SerializeField] private GameObject inventoryItemPrefab;

    [Header("Editor")]
    public ItemSO commandAddItem;

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Q))
        {
            AddItem(commandAddItem);
        }
#endif
    }

    public bool AddItem(ItemSO item)
    {
        InventorySlot emptySlot = null;
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventoryItem itemInSlot = inventorySlots[i].GetComponentInChildren<InventoryItem>();
            if (itemInSlot == null && emptySlot == null)
            {
                emptySlot = inventorySlots[i];
            }
            else if (itemInSlot != null && itemInSlot.item == item && itemInSlot.count < maxStackItem && itemInSlot.item.stackable)
            {
                itemInSlot.count++;
                itemInSlot.RefreshCount();
                return true;
            }
        }
        if (emptySlot != null)      // 아이템이 있던건 아니여서 빈 슬롯에 넣자.
        {
            SpawnNewItem(item, emptySlot);      // 빈 슬롯을 찾았다면
            return true;
        }
        return false;
    }

    private void SpawnNewItem(ItemSO item, InventorySlot slot)
    {
        GameObject newItem = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItem.GetComponent<InventoryItem>();
        inventoryItem.InitItem(item);
    }

    public int ItemCoundCheck(ItemSO findItem)
    {
        int count = 0;
        foreach (InventorySlot slot in inventorySlots)
        {
            InventoryItem inventoryItem = slot.GetItemSO();
            if (inventoryItem != null && inventoryItem.item == findItem)
            {
                count += inventoryItem.count;
            }
        }

        return count;
    }

    public void UseItem(ItemSO useItem, int count)
    {
        foreach (InventorySlot slot in inventorySlots)
        {
            InventoryItem inventoryItem = slot.GetItemSO();
            if (inventoryItem != null && inventoryItem.item == useItem)
            {
                inventoryItem.count -= count;
                if (inventoryItem.count <= 0)
                {
                    Destroy(inventoryItem.gameObject);      // 풀링하기!!
                    count = Mathf.Abs(inventoryItem.count);
                }
                else
                {
                    inventoryItem.RefreshCount();
                }
            }
        }
    }
}
