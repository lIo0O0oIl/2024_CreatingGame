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
            instance = obj.AddComponent<GameManager>();     // AddComponent 는 리플렉션이라 성능 안좋음..
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

    public void InputActiveSetting(bool is_Active)              // 이건 아님 아님.  코드 나중에 수정하기.
    {
        playerController.InputActiveSetting(is_Active);
    }

}
