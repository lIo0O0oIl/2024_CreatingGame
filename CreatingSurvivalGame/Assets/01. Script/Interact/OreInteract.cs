using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OreInteract : MonoBehaviour, IPlayerInteract
{
    [SerializeField] private InventoryManager inventoryManager;
    [SerializeField] private ItemSO[] dropItem;
    [SerializeField] private int mySlotNum = 2;     // 곡괭이

    public bool Interact(int slotNum)
    {
        if (slotNum != mySlotNum)
        {
            Debug.Log("다른 도구 들어");
            return false;
        }

        float[] persent = new float[dropItem.Length];
        float totalPersent = 0.0f;
        float settingPersent = 0.5f;      // 첫번째 아이템은 50%

        for (int i = 0; i < dropItem.Length - 2; i++)
        {
            persent[i] = settingPersent;
            totalPersent += settingPersent;
            settingPersent /= 2;        // 절반으로 설정
        }

        float remainPerent = 1 - totalPersent;
        float secondLastPersent = remainPerent * 2 / 3;
        float lastPersent = remainPerent * 1 / 3;
        float random = Random.value;        // 0 ~ 1;

        float currentPersent = 0.0f;
        for (int i = 0;i < dropItem.Length;i++)
        {
            if (i == dropItem.Length - 2) currentPersent += secondLastPersent;
            else if (i == dropItem.Length - 1) currentPersent += lastPersent;
            else currentPersent += persent[i];

            if (currentPersent >=  random)
            {
                bool result = inventoryManager.AddItem(dropItem[i]);
                if (result == false)
                {
                    Debug.LogError("아이템창 꽉 참");
                    return false;
                }
                GameManager.Instance.StatisticsManager.OneAddStatistic(Statistics.Interact);
                return true;
            }
        }
        return false;
    }
}
