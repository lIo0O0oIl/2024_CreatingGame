using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private ItemSO item;

    [HideInInspector] public Transform afterDragTrm;
    private Image myImage;

    private void Awake()
    {
        myImage = GetComponent<Image>();
    }

    public void InitItem(ItemSO newItem)
    {
        item = newItem;
        myImage.sprite = item.sprite;
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
