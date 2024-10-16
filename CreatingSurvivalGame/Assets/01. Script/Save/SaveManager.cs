using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class SaveData
{
    public Vector3 playerPos;
    public int playerHp;
    public bool enemyDie;           // ���߿� Ȯ���Ϸ��� �迭�� �ؼ� �����
    public bool[] vesselClear = new bool[3];        // 3���� ���� ���翣
    public SaveInventorySlot[] saveInventorySlots = new SaveInventorySlot[20];      // ���� ���� 20����.
    public int[] statistics = new int[(int)Statistics.End];
}

[Serializable]
public struct SaveInventorySlot
{
    public bool is_Use;
    public string itemName;
    public int itemCount;
}

public class SaveManager : MonoBehaviour
{
    private SaveData saveData = new SaveData();
    private string savePath;

    // �ʿ��� ��ũ��Ʈ�� ��������
    [SerializeField] private GameObject playerObj;       // ���ӸŴ������� �����͵� ��.
    private PlayerStat playerStat;       // ���ӸŴ������� �����͵� ��.
    [SerializeField] private EnemyManager enemyManager;
    [SerializeField] private InventoryManager inventoryManager;
    [SerializeField] private StatisticsManager statisticsManager;
    [SerializeField] private VesselInteract[] vessels = new VesselInteract[3];

    private void Start()
    {
        playerStat = playerObj.GetComponent<PlayerStat>();
        savePath = Application.dataPath + "/SaveData";
        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }
    }

    public void Save()
    {
        saveData.playerPos = playerObj.transform.position;
        saveData.playerHp = playerStat.Currenthp;
        saveData.enemyDie = enemyManager.aiBrains[0].Is_Death;       // �ϴ� 1���ϱ�.
        for (int i = 0; i < vessels.Length; i++)
        {
            saveData.vesselClear[i] = vessels[i].is_Ok;
        }
        for (int i = 0; i < inventoryManager.inventorySlots.Length; i++)
        {
            InventoryItem item = inventoryManager.inventorySlots[i].GetItemSO();
            if (item != null)
            {
                saveData.saveInventorySlots[i].is_Use = true;
                saveData.saveInventorySlots[i].itemName = item.item.itemName;
                saveData.saveInventorySlots[i].itemCount = item.count;
            }
            else
            {
                saveData.saveInventorySlots[i].is_Use = default;
                saveData.saveInventorySlots[i].itemName = default;          // == null
                saveData.saveInventorySlots[i].itemCount = default;
            }
        }
        saveData.statistics = statisticsManager.GetStatisticsArray();

        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(savePath + "/SaveFile.txt", json);
#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif
    }

    public void Load()
    {
        if (File.Exists(savePath + "/SaveFile.txt"))
        {
            string LoadJson = File.ReadAllText(savePath + "/SaveFile.txt");
            saveData = JsonUtility.FromJson<SaveData>(LoadJson);

            // �ҷ��� ������ ������� �ҷ��� ������ �ε�.
            playerObj.transform.position = saveData.playerPos;
            playerStat.SetHp(saveData.playerHp);
            if (saveData.enemyDie) enemyManager.EnemyDie(0);        // �ϴ� �Ѹ����ϱ�.
            for (int i = 0; i < saveData.vesselClear.Length; i++)
            {
                if (saveData.vesselClear[i]) vessels[i].VesselClear();
            }
            for (int i = 0; i < saveData.saveInventorySlots.Length; i++)
            {
                SaveInventorySlot itemSlotInfo = saveData.saveInventorySlots[i];
                ItemSO loadItem = inventoryManager.GetLoadItemFind(itemSlotInfo.itemName);
                if (loadItem != null)
                {
                    inventoryManager.LoadAndSpawnItem(loadItem, itemSlotInfo.itemCount, i);
                }
            }
            statisticsManager.LoadStatistics(saveData.statistics);
        }
        else Debug.LogError("���� ���� ����");
    }
}


/*
���� �ý���
1. ���� ������ ������ ������. ���� ������ �����̶� ������ ����.
- ���� �� �͵� : �÷��̾� ��ġ, �κ��丮, �Լ� Ŭ���� ���
                          ���� �׾����� �Ǵ�(������ ���� �ϳ��ϱ� ����)

2. ��Ʈ�ο����� �����Ӱ� �ε��ϴ� ��ư�� ����.
- �ε� ��ư�� ���̺� ������ ���� ���� ����.
- �� ������ ���� �� �����ϴ� ���̺� ���� ����
 
 // ��Ʈ�� ���� UI �Ŵ���? �־ ���� ������ ���̰� �ϴ� �� ������ ������.
���� �ִ°� �Ἥ �ϸ� �� �� ����. 
�ε��� ���� ������ ��� ����
�׳� ĵ������ ���� �÷�����. 
 */
