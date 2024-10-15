using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStat : MonoBehaviour, IHitInterface
{
    [Header("Stat")]
    [SerializeField] private int damage = 30;
    public int myDamage => damage;
    [SerializeField] private int maxHp = 100;
    private int currentHp;

    [Header("Show")]
    [SerializeField] private GameObject hpbar;
    [SerializeField] private TMP_Text hpbarText;


    private PlayerAnimation playerAnim;

    private void Awake()
    {
        playerAnim = GetComponent<PlayerAnimation>();
        currentHp = maxHp;
    }

    public void Damage(int damge)
    {
        currentHp -= damge;

        if (currentHp <= 0)
        {
            playerAnim.SetDeath(true);
            hpbar.transform.localScale = new Vector3(0, 1, 1);
            hpbarText.text = $"0/{maxHp}";

            GameManager.Instance.GameOver();
            return;
        }

        hpbar.transform.localScale = new Vector3(currentHp * 0.01f, 1, 1);
        hpbarText.text = $"{currentHp}/{maxHp}";

    }
}
