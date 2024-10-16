using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public AIBrain[] aiBrains;

    public void AllEnemyDie()       // 게임 오버일 때 다 죽임.
    {
        foreach (var brain in aiBrains)
        {
            brain.Death();
        }
    }

    public void EnemyDie(int index)     // 게임 로드했을 때 죽일 몬스터들
    {
        aiBrains[index].Death(false);       // 템 적용은 하지마.
    }
}
