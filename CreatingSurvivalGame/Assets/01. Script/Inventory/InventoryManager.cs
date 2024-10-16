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
                emptySlot = inventorySlots[i];      // ����ִ� ���� ���� ����
            }
            else if (itemInSlot != null && itemInSlot.item == item && itemInSlot.count < maxStackItem && itemInSlot.item.stackable)
            {
                itemInSlot.count++;
                itemInSlot.RefreshCount();
                return true;
            }
        }
        if (emptySlot != null)      // �������� �ִ��� �ƴϿ��� �� ���Կ� ����.
        {
            SpawnNewItem(item, emptySlot);      // �� ������ ã�Ҵٸ�
            return true;
        }
        return false;
    }

    private void SpawnNewItem(ItemSO item, InventorySlot slot)      // ������ ���� ����
    {
        GameObject newItem = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItem.GetComponent<InventoryItem>();
        inventoryItem.InitItem(item);
    }

    public int ItemCoundCheck(ItemSO findItem)      // ������ ���� ��������
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

    public bool UseItem(ItemSO useItem, int count)      // ������ ����ϱ�
    {
        bool useOk = false;

        foreach (InventorySlot slot in inventorySlots)
        {
            InventoryItem inventoryItem = slot.GetItemSO();
            if (inventoryItem != null && inventoryItem.item == useItem)
            {
                useOk = true;
                inventoryItem.count -= count;
                if (inventoryItem.count <= 0)           // ���� �κ��丮�� �ִ� ī��Ʈ�� 0�̰ų� ������
                {
                    Destroy(inventoryItem.gameObject);      // Ǯ���ϱ�!!
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

    public ItemSO GetLoadItemFind(string name)       // ������ �ε� �ҷ���
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
