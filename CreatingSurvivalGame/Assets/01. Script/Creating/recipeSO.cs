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
    public NeedItem[] needItem;     // 최대 3개까지
    public ItemSO results; 
}
