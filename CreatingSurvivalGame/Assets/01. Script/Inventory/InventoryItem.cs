using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public ItemSO item;
    [HideInInspector] public int count = 1;
    [HideInInspector] public Transform afterDragTrm;

    [SerializeField] private Image myImage;
    [SerializeField] private TMP_Text myCountText;

    private void Awake()
    {
        if (myImage == null && myCountText == null)
        {
            Debug.LogError("UI 인스펙터에서 넣어주세용");
        }
    }

    public void InitItem(ItemSO newItem)
    {
        item = newItem;
        myImage.sprite = item.sprite;
        RefreshCount();
    }

    public void RefreshCount()
    {
        myCountText.text = count.ToString();
        bool textAtive = count > 1;
        myCountText.gameObject.SetActive(textAtive);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        afterDragTrm = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();           // 가장 마지막 순서로 배치
        myImage.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(afterDragTrm);
        transform.localPosition = Vector2.zero;
        myImage.raycastTarget = true;
    }
}
