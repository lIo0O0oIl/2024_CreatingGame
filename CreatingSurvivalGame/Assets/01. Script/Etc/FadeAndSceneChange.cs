using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeAndSceneChange : MonoBehaviour
{
    private Image fadeImage;
    [SerializeField] private float daleyTime;
    [SerializeField] private int sceneIndex;

    private WaitForSeconds daley;

    private void Awake()
    {
        fadeImage = GetComponent<Image>();
        daley = new WaitForSeconds(daleyTime);
    }

    void Start()
    {
        fadeImage.DOFade(1, 2);
        StartCoroutine(SceneChange());
    }

    private IEnumerator SceneChange()
    {
        yield return daley;
        SceneManager.LoadScene(sceneIndex);      // intro°¡ÀÚ.
    }
}
