using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUseTool : MonoBehaviour
{
    // �÷��̾ ���� ���õȰ� ����� �˷��ְ� ����ϴ� �뵵.
    public int currentSelectNum = 1;
    [SerializeField] private Image[] toolsSlot = new Image[3];      // index ���°� ���� ����
    [SerializeField] private Color selectedColor, nomalColor;

    private PlayerController controller;

    private void Awake()
    {
        controller = GetComponent<PlayerController>();
        SelectSlot(currentSelectNum);
    }

    private void OnEnable()
    {
        controller.SelectSlotAction += SelectSlot;
    }

    private void OnDisable()
    {
        controller.SelectSlotAction -= SelectSlot;
    }

    private void SelectSlot(int slotNum)
    {
        toolsSlot[currentSelectNum - 1].color = nomalColor;
        currentSelectNum = slotNum;
        toolsSlot[currentSelectNum - 1].color = selectedColor;
    }    
}
