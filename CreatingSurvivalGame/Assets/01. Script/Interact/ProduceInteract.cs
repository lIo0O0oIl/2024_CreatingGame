using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProduceInteract : MonoBehaviour, IPlayerInteract
{
    [Header("UI")]
    [SerializeField] private GameObject produceUI;
    [SerializeField] private CreatingManager creatingManager;

    public bool Interact(int slotNum)       // ¾ê´Â ½½·Ô³Ñ¹ö »ó°ü¾øÀ½.
    {
        GameManager.Instance.InputActiveSetting(false);

        creatingManager.ChangeCategory(1);
        produceUI.SetActive(true);

        return true;
    }
}
