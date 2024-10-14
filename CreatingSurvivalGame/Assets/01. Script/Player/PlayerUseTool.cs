using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUseTool : MonoBehaviour
{
    // 플레이어가 지금 선택된게 어떤건지 알려주고 사용하는 용도.
    public int currentSelectNum = 1;
    [SerializeField] private Image[] toolsSlot = new Image[3];      // index 쓰는거 잊지 말기
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
