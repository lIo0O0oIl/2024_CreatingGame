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
            instance = obj.AddComponent<GameManager>();     // AddComponent �� ���÷����̶� ���� ������..
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
        if (playerController == null) { Debug.Log("�÷��̾� ��Ʈ�ѷ� ����"); }

        wait = new WaitForSeconds(gameOverDelayTime);
    }

    public void InputActiveSetting(bool is_Active)              // �̰� �ƴ� �ƴ�.  �ڵ� ���߿� �����ϱ�.
    {
        playerController.InputActiveSetting(is_Active);
    }

    public void GameOver(bool is_over = true)
    {
        Debug.Log("���� ����");
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
            // ���� UI ���ֱ�
            gameOverUI.SetActive(true);
        }
        else
        {
            // Ŭ���� UI ���ֱ�
            //gameClearUI.SetActive(true);
            gameOverUI.SetActive(true);         // �ϴ� �׳� �� ������
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
