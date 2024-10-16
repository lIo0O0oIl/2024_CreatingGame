using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public AIBrain[] aiBrains;

    public void AllEnemyDie()       // ���� ������ �� �� ����.
    {
        foreach (var brain in aiBrains)
        {
            brain.Death();
        }
    }

    public void EnemyDie(int index)     // ���� �ε����� �� ���� ���͵�
    {
        aiBrains[index].Death(false);       // �� ������ ������.
    }
}
