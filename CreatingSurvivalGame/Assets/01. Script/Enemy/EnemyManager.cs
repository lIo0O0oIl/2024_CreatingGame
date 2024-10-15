using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private AIBrain[] aiBrains;

    public void AllEnemyDie()
    {
        foreach (var brain in aiBrains)
        {
            brain.Death();
        }
    }
}
