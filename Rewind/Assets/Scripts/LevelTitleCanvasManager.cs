using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class LevelTitleCanvasManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI tmp;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    private void OnLevelWasLoaded(int level)
    {
        string lvlName = SceneManager.GetSceneByBuildIndex(level).name;
        tmp.text = lvlName;

        tmp.DOFade(1, 2).OnComplete(() => StartCoroutine(FadeOutCoroutine()));
    }

    private IEnumerator FadeOutCoroutine()
    {
        yield return new WaitForSeconds(2f);
        tmp.DOFade(0, 1);
    }
}
