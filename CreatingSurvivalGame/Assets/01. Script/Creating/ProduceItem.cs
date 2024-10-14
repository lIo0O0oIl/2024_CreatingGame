using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProduceItem : MonoBehaviour
{
    public recipeSO recipe;
    [SerializeField] private Image myImage;
    [SerializeField] private Button myButton;

    private void Awake()
    {
        if (myImage == null)
        {
            Debug.LogError("UI 인스펙터에서 넣어주세용");
        }
    }

    public void RecipeClear()
    {
        recipe = null;
        myImage.sprite = null;
        myImage.enabled = false;
        myButton.enabled = false;
    }

    public void ChangeItem(recipeSO newRecipe)
    {
        myImage.enabled = true;
        myButton.enabled = true;
        recipe = newRecipe;
        myImage.sprite = recipe.results.sprite;
    }
}
