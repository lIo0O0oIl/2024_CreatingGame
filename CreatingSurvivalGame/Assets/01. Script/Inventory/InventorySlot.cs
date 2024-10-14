using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.TryGetComponent<InventoryItem>(out InventoryItem item))     // 드랍된거 가져오기
        {
            if (transform.childCount > 0)       // 만약 자식이 있으면 스왑
            {
                Transform myItem = transform.GetChild(0);
                myItem.SetParent(item.afterDragTrm);
                myItem.localPosition = Vector2.zero;
                myItem.GetComponent<InventoryItem>().afterDragTrm = myItem.parent;
            }

            item.afterDragTrm = transform;
        }
    }
}
