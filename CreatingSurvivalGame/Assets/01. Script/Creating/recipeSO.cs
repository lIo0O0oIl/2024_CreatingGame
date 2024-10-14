using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct NeedItem
{
    public ItemSO item;
    public int count;
}

[CreateAssetMenu(menuName = "SO/Recipe")]
public class recipeSO : ScriptableObject
{
    public NeedItem[] needItem;     // �ִ� 3������
    public ItemSO results; 
}
