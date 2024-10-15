using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStat : MonoBehaviour, IHitInterface
{
    [Header("Stat")]
    [SerializeField] private int damage = 30;
    public int myDamage => damage;
    [SerializeField] private int maxHp = 100;

    private int currentHp;

    [Header("Show")]
    [SerializeField] private Slider hpbar;

    private AIBrain brain;

    private void Awake()
    {
        brain = GetComponent<AIBrain>();
        currentHp = maxHp;
    }

    public void Damage(int damge)
    {
        currentHp -= damge;
        hpbar.value = currentHp;

        if (currentHp <= 0)
        {
            brain.Death();
        }
    }
}
