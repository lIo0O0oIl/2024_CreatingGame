using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VesselInteract : MonoBehaviour, IPlayerInteract
{
    [SerializeField] private ItemSO needItem;

    [SerializeField] private GameObject showObj;     // 보여질 이미지
    [SerializeField] private GameObject exampleObj;     // 지워질 이미지. 이미 있던 애. 아마 사용?

    [HideInInspector] public bool is_Ok = false;     // 찾아졌으면 이거 바뀔 때마다 이벤트 호출하게

    private void Start()
    {
        showObj.SetActive(false);
    }

    public void Interact(int slotNum)
    {
        Debug.Log("확인");

        // 이미 된거라면 리턴.
        if (is_Ok) return;

        // 인벤토리에서 찾기
        if (GameManager.Instance.InventoryManager.UseItem(needItem, 1))
        {
            is_Ok = true;
            showObj.SetActive(true);
            exampleObj.SetActive(false);
            GameManager.Instance.ClearCheck();
        }
    }
}
