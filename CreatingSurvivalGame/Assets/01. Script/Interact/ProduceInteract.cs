using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProduceInteract : MonoBehaviour, IPlayerInteract
{
    [Header("UI")]
    [SerializeField] private GameObject produceUI;

    public void Interact(int slotNum)       // ��� ���Գѹ� �������.
    {
        produceUI.SetActive(true);
    }
}
