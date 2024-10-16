using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntroUIManager : MonoBehaviour     // 캔버스에 위치해두게 하기
{
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject loadBtn;
    [SerializeField] private GameObject fadeObj;
    private Image fadeImage;

    [SerializeField] private float fadeDuration;
    private WaitForSeconds delay;

    #region Singleton
    private static IntroUIManager instance;
    /*    public static IntroUIManager Instance
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
            instance = FindObjectOfType<IntroUIManager>();
            if (instance == null)
            {
                GameObject obj = new GameObject();
                obj.name = "IntroUIManager";
                instance = obj.AddComponent<IntroUIManager>();     // AddComponent 는 리플렉션이라 성능 안좋음..
                DontDestroyOnLoad(obj);
            }
        }*/         // 전역으로 호출될거는 아님.

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
            instance.Start();
        }

        fadeImage = fadeObj.GetComponent<Image>();
        delay = new WaitForSeconds(fadeDuration);
    }
    #endregion

    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)      // Intro 이면
        {
            canvas.SetActive(true);

            if (File.Exists(Application.dataPath + "/SaveData/SaveFile.txt"))
            {
                loadBtn.SetActive(true);
            }
            else
            {
                loadBtn.SetActive(false);
            }
        }
        else
        {
            canvas.SetActive(false);
        }
    }

    public void ClickStart()
    {
        StartCoroutine(Load(1));
        if (File.Exists(Application.dataPath + "/SaveData/SaveFile.txt"))
        {
            File.Delete(Application.dataPath + "/SaveData/SaveFile.txt");       // 새 게임이니까 지워주기
        }
    }

    public void ClickLoad()
    {
        StartCoroutine(Load(1, true));
    }

    private IEnumerator Load(int loadIndex, bool is_Load = false)
    {
        fadeObj.SetActive(true);
        fadeImage.DOFade(1, fadeDuration);
        yield return delay;

        AsyncOperation operation = SceneManager.LoadSceneAsync(loadIndex);
        while (!operation.isDone)
        {
            yield return null;
        }

        if (is_Load == true)
        {
            SaveManager saveManager = FindObjectOfType<SaveManager>();
            saveManager.Load();
        }

        Color fadeColor = fadeImage.color;
        fadeColor.a = 0;
        fadeImage.color = fadeColor;
        fadeObj.SetActive(false);

        Start();
    }

    public void ClickStory()
    {
        StartCoroutine(Load(2));
    }

    public void ClickExit()
    {
        Application.Quit();
    }
}
