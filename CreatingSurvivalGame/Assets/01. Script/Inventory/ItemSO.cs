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
    //public AddStatSO addStat;           // ���� ��� ���� SO �־ ���ֱ�.

    [Header("ETC")]
    public bool stackable = true;
}

public enum ItmeType
{
    Nomal,
    Eqipment,
}