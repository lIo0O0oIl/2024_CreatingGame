using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                SetUpInstance();
            }
            return instance;
        }
    }

    private static void SetUpInstance()
    {
        instance = FindObjectOfType<GameManager>();
        if (instance == null)
        {
            GameObject obj = new GameObject();
            obj.name = "GameManager";
            instance = obj.AddComponent<GameManager>();     // AddComponent �� ���÷����̶� ���� ������..
            //DontDestroyOnLoad(obj);
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    #endregion

    public PlayerController playerController;

    public void InputActiveSetting(bool is_Active)              // �̰� �ƴ� �ƴ�.  �ڵ� ���߿� �����ϱ�.
    {
        playerController.InputActiveSetting(is_Active);
    }

}
