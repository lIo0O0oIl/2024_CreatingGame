using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeInteract : MonoBehaviour, IPlayerInteract
{
    [SerializeField] private InventoryManager inventoryManager;
    [SerializeField] private ItemSO dropItem;
    [SerializeField] private int mySlotNum = 1;     // ����

    public bool Interact(int slotNum)
    {
        if (slotNum != mySlotNum)
        {
            Debug.Log("�ٸ� ���� ���");
            return false;
        }
        
        bool result = inventoryManager.AddItem(dropItem);
        if (result == false)
        {
            Debug.LogError("������â �� ��");
            return false;
        }
        return true;
    }
}
