using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="SO/Item")]
public class ItemSO : ScriptableObject
{
    [Header("Game")]
    public string itemName;
    public Sprite sprite;
    public ItmeType type;
    //public AddStatSO addStat;           // 장비면 장비 스텟 SO 넣어서 해주기.

    [Header("ETC")]
    public bool stackable = true;
}

public enum ItmeType
{
    Nomal,
    Eqipment,
}