using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum Statistics
{
    Walk,
    Jump,
    Attack,
    Hit,
    Interact,
    ItemMake,
    //Time
    End
}

public class StatisticsManager : MonoBehaviour
{
    [SerializeField] private TMP_Text[] countText;
    private Dictionary<Statistics, int> statisticsDic = new Dictionary<Statistics, int>();

    private void Awake()
    {
        var values = Enum.GetValues(typeof(Statistics));        // 리플렉션. 느려요.
        for (int i = 0; i < (int)Statistics.End; i++)
        {
            statisticsDic.Add((Statistics)values.GetValue(i), 0);
        }
    }

    public void OneAddStatistic(Statistics thisAdd)
    {
        statisticsDic[thisAdd]++;
    }

    public void ItemMakeBtnClick()
    {
        statisticsDic[Statistics.ItemMake]++;
    }

    public void TextRefresh()
    {
        int[] nums = GetStatisticsArray();
        for (int i = 0;i < countText.Length; i++)
        {
            countText[i].text = nums[i].ToString();
        }
    }

    public int[] GetStatisticsArray()
    {
        int[] nums = new int[statisticsDic.Count];
        var values = Enum.GetValues(typeof(Statistics));        // 리플렉션
        for (int i = 0; i < (int)Statistics.End; i++)
        {
            nums[i] = statisticsDic[(Statistics)values.GetValue(i)];
        }
        return nums;
    }

    public void LoadStatistics(int[] statisticsNum)
    {
        var values = Enum.GetValues(typeof(Statistics));        // 리플렉션
        for (int i = 0; i < (int)Statistics.End; i++)
        {
            statisticsDic[(Statistics)values.GetValue(i)] = statisticsNum[i];
        }
    }
}

// 이넘 쓰지 말걸. 리플렉션 너무 함. 다른 구조 찾아서 하기.....