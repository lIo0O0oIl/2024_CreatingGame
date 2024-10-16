using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private int maxStackItem = 64;
    public InventorySlot[] inventorySlots;
    [SerializeField] private GameObject inventoryItemPrefab;
    [SerializeField] private ItemSO[] allItemArray;

    [Header("Editor")]
    public ItemSO commandAddItem;

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (commandAddItem != null) AddItem(commandAddItem);
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
                emptySlot = inventorySlots[i];      // 비어있는 가장 빠른 슬롯
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

    private void SpawnNewItem(ItemSO item, InventorySlot slot)      // 아이템 직접 생성
    {
        GameObject newItem = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItem.GetComponent<InventoryItem>();
        inventoryItem.InitItem(item);
    }

    public int ItemCoundCheck(ItemSO findItem)      // 아이템 개수 가져오기
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

    public bool UseItem(ItemSO useItem, int count)      // 아이템 사용하기
    {
        bool useOk = false;

        foreach (InventorySlot slot in inventorySlots)
        {
            InventoryItem inventoryItem = slot.GetItemSO();
            if (inventoryItem != null && inventoryItem.item == useItem)
            {
                useOk = true;
                inventoryItem.count -= count;
                if (inventoryItem.count <= 0)           // 지금 인벤토리에 있는 카운트가 0이거나 작으면
                {
                    Destroy(inventoryItem.gameObject);      // 풀링하기!!
                    count = Mathf.Abs(inventoryItem.count);
                }
                else
                {
                    inventoryItem.RefreshCount();
                    break;
                }
            }
        }

        return useOk;
    }

    public void LoadAndSpawnItem(ItemSO item, int count, int slotIndex)
    {
        GameObject loadItem = Instantiate(inventoryItemPrefab, inventorySlots[slotIndex].transform);
        InventoryItem inventoryItem = loadItem.GetComponent<InventoryItem>();
        inventoryItem.InitItem(item, count);
    }

    public ItemSO GetLoadItemFind(string name)       // 아이템 로드 할려고
    {
        foreach (ItemSO item in allItemArray)
        {
            if (item.itemName == name)
            {
                return item;
            }
        }

        return null;
    }
}
