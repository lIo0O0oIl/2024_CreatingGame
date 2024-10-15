using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                SetUpInstance();
            }
            return instance;
        }
    }

    private static void SetUpInstance()
    {
        instance = FindObjectOfType<GameManager>();
        if (instance == null)
        {
            GameObject obj = new GameObject();
            obj.name = "GameManager";
            instance = obj.AddComponent<GameManager>();     // AddComponent 는 리플렉션이라 성능 안좋음..
            //DontDestroyOnLoad(obj);
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    #endregion

    public PlayerController playerController;
    private EnemyManager enemyManager;
    private InventoryManager inventoryManager;
    public InventoryManager InventoryManager => inventoryManager;

    [SerializeField] private ItemSO[] giveEnemyItem;

    [Header("GameOver")]
    [SerializeField] private float gameOverDelayTime = 3f;
    private WaitForSeconds wait;
    [SerializeField] private GameObject gameOverUI;

    [Header("GameClear")]
    [SerializeField] private VesselInteract[] vesselInteracts;
    //[SerializeField] private GameObject gameClearUI;

    private void Start()
    {
        enemyManager = GetComponent<EnemyManager>();
        inventoryManager = GetComponent<InventoryManager>();
        if (playerController == null) { Debug.Log("플레이어 컨트롤러 없음"); }

        wait = new WaitForSeconds(gameOverDelayTime);
    }

    public void InputActiveSetting(bool is_Active)              // 이건 아님 아님.  코드 나중에 수정하기.
    {
        playerController.InputActiveSetting(is_Active);
    }

    public void GameOver(bool is_over = true)
    {
        Debug.Log("게임 오버");
        playerController.playerInputs.Player.Movement.Disable();
        playerController.playerInputs.Player.SelectSlot.Disable();
        playerController.playerInputs.Player.Attack.Disable();

        StartCoroutine(EnemyAllDie());
    }

    private IEnumerator EnemyAllDie(bool is_over = true)
    {
        yield return wait;
        enemyManager.AllEnemyDie();

        if (is_over)
        {
            // 오버 UI 켜주기
            gameOverUI.SetActive(true);
        }
        else
        {
            // 클리어 UI 켜주기
            //gameClearUI.SetActive(true);
            gameOverUI.SetActive(true);         // 일단 그냥 다 오버로
        }
    }

    public void GiveEnemyItem()
    {
        foreach (var item in giveEnemyItem)
        {
            inventoryManager.AddItem(item);
        }
    }

    public void ClearCheck()
    {
        foreach (var vessel in vesselInteracts)
        {
            if (vessel.is_Ok == false) return;
        }

        GameOver(false);
    }
}
