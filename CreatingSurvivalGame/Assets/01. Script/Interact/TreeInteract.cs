using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeInteract : MonoBehaviour, IPlayerInteract
{
    [SerializeField] private InventoryManager inventoryManager;
    [SerializeField] private ItemSO dropItem;
    [SerializeField] private int mySlotNum = 1;     // 도끼

    public bool Interact(int slotNum)
    {
        if (slotNum != mySlotNum)
        {
            Debug.Log("다른 도구 들어");
            return false;
        }
        
        bool result = inventoryManager.AddItem(dropItem);
        if (result == false)
        {
            Debug.LogError("아이템창 꽉 참");
            return false;
        }
        return true;
    }
}
