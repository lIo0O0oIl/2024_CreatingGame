using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MaterialSlot : MonoBehaviour
{
    [SerializeField] private Image myImage;
    [SerializeField] private TMP_Text myText;

    private Color redColor = Color.red;
    private Color GreenColor = Color.green;

    public void Clear()
    {
        myImage.enabled = false;
        myText.enabled = false;
    }

    public void ChangeSlot(Sprite sprite, int count, int invenItemCount)
    {
        myImage.enabled = true;
        myText.enabled = true;

        myImage.sprite = sprite;

        if (count <= invenItemCount) myText.color = GreenColor;
        else myText.color = redColor;
        myText.text = $"{invenItemCount}/{count}";
    }
}
