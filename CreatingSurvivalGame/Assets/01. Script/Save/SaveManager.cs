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
    public bool enemyDie;           // 나중에 확장하려면 배열로 해서 만들기
    public bool[] vesselClear = new bool[3];        // 3개만 있음 현재엔
    public SaveInventorySlot[] saveInventorySlots = new SaveInventorySlot[20];      // 지금 슬롯 20개임.
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

    // 필요한 스크립트들 가져오기
    [SerializeField] private GameObject playerObj;       // 게임매니져에서 가져와도 됨.
    private PlayerStat playerStat;       // 게임매니져에서 가져와도 됨.
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
        saveData.enemyDie = enemyManager.aiBrains[0].Is_Death;       // 일단 1개니까.
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

            // 불러온 데이터 기반으로 불러온 데이터 로드.
            playerObj.transform.position = saveData.playerPos;
            playerStat.SetHp(saveData.playerHp);
            if (saveData.enemyDie) enemyManager.EnemyDie(0);        // 일단 한마리니까.
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
        else Debug.LogError("저장 파일 없음");
    }
}


/*
저장 시스템
1. 게임 씬에서 저장이 가능함. 설정 누르면 저장이랑 나가기 있음.
- 저장 할 것들 : 플레이어 위치, 인벤토리, 함선 클리어 요소
                          적이 죽었는지 판단(지금은 적이 하나니까 가능)

2. 인트로에서는 새게임과 로드하는 버튼이 있음.
- 로드 버튼은 세이브 파일이 있을 때만 보임.
- 새 게임을 누를 시 존재하던 세이브 파일 삭제
 
 // 인트로 저장 UI 매니져? 있어서 파일 있으면 보이게 하는 게 있으면 좋을듯.
위에 있는거 써서 하면 될 것 같고. 
로드할 때를 구현을 어떻게 하지
그냥 캔버스를 위에 올려두자. 
 */
