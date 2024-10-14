using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private int maxStackItem = 64;
    [SerializeField] private InventorySlot[] inventorySlots;
    [SerializeField] private GameObject inventoryItemPrefab;

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

            if (emptySlot != null)      // �������� �ִ��� �ƴϿ��� �� ���Կ� ����.
            {
                SpawnNewItem(item, emptySlot);      // �� ������ ã�Ҵٸ�
                return true;
            }
        }
        return false;
    }

    private void SpawnNewItem(ItemSO item, InventorySlot slot)
    {
        GameObject newItem = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItem.GetComponent<InventoryItem>();
        inventoryItem.InitItem(item);
    }
}
