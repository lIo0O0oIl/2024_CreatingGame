using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProduceInteract : MonoBehaviour, IPlayerInteract
{
    [Header("UI")]
    [SerializeField] private GameObject produceUI;

    public void Interact(int slotNum)       // ¾ê´Â ½½·Ô³Ñ¹ö »ó°ü¾øÀ½.
    {
        produceUI.SetActive(true);
    }
}
