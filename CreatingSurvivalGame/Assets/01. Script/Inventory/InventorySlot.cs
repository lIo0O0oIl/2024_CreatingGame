using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.TryGetComponent<InventoryItem>(out InventoryItem item))     // ����Ȱ� ��������
        {
            if (transform.childCount > 0)       // ���� �ڽ��� ������ ����
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
