using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VesselInteract : MonoBehaviour, IPlayerInteract
{
    [SerializeField] private ItemSO needItem;

    [SerializeField] private GameObject showObj;     // ������ �̹���
    [SerializeField] private GameObject exampleObj;     // ������ �̹���. �̹� �ִ� ��. �Ƹ� ���?

    [HideInInspector] public bool is_Ok = false;     // ã�������� �̰� �ٲ� ������ �̺�Ʈ ȣ���ϰ�

    private void Start()
    {
        showObj.SetActive(false);
    }

    public void Interact(int slotNum)
    {
        Debug.Log("Ȯ��");

        // �̹� �ȰŶ�� ����.
        if (is_Ok) return;

        // �κ��丮���� ã��
        if (GameManager.Instance.InventoryManager.UseItem(needItem, 1))
        {
            is_Ok = true;
            showObj.SetActive(true);
            exampleObj.SetActive(false);
            GameManager.Instance.ClearCheck();
        }
    }
}
