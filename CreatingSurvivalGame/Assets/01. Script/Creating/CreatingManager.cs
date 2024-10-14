using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct MakeCategory
{
    public recipeSO[] items;
}

public class CreatingManager : MonoBehaviour
{
    [SerializeField] private MakeCategory[] makeList;
    [SerializeField] private ProduceItem[] produceItems;
    [SerializeField] private MaterialSlot[] materialSlots = new MaterialSlot[3];

    [Header("UI")]
    [SerializeField] private Button[] buttonList = new Button[3];
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private Button makeBtn;
    
    [SerializeField] private int currentSelectIndex = 1;

    private InventoryManager inventoryManager;
    private recipeSO currentRecipe;

    private void Awake()
    {
        inventoryManager = GetComponent<InventoryManager>();
        ChangeCategory(currentSelectIndex);
    }

    public void ChangeCategory(int changeCategoryIndex)
    {
        foreach (var button in buttonList) button.interactable = true;
        buttonList[changeCategoryIndex].interactable = false;

        foreach (var slot in produceItems)      // 전부 지워주고 섹액티브 다 꺼
        {
            slot.RecipeClear();
        }


        int slotIndex = 0;
        foreach (var recipeSO in makeList[changeCategoryIndex].items)     // 새로운 아이템으로 변경
        {
            produceItems[slotIndex].ChangeItem(recipeSO);
            slotIndex++;
        }

        ItemRecipeShow(produceItems[0].recipe);
    }

    public void OnShow(ProduceItem produceItem)     // Btn 
    {
        Debug.Log("클릭됨");

        recipeSO recipe = produceItem.recipe;
        ItemRecipeShow(recipe);
    }

    private void ItemRecipeShow(recipeSO recipe)
    {
        currentRecipe = recipe;

        foreach (var slot in materialSlots)
        {
            slot.Clear();
        }
        makeBtn.interactable = true;

        int materialSlotIndex = 0;
        foreach (var needItem in currentRecipe.needItem)
        {
            int inventItemCount = inventoryManager.ItemCoundCheck(needItem.item);
            materialSlots[materialSlotIndex].ChangeSlot(needItem.item.sprite, needItem.count, inventItemCount);        //인벤토리에서 개수 가져오기

            if (inventItemCount < needItem.count)
            {
                makeBtn.interactable = false;
            }

            materialSlotIndex++;
        }

        descriptionText.text = $"{currentRecipe.results.itemName}. {currentRecipe.results.description}";
    }

    public void OnMakeBtn()
    {
        Debug.Log("제작 가능");
        // 빼기
        foreach (var item in currentRecipe.needItem)
        {
            inventoryManager.UseItem(item.item, item.count);
        }

        inventoryManager.AddItem(currentRecipe.results);
        // 만들어줌.
        ItemRecipeShow(currentRecipe);      // 다시 보이게
    }
}
