using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class SoundtrackManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource as1;
    [SerializeField]
    private AudioSource as2;

    public float fadeDuration;
    public float volume;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        as1.volume = volume;
        as2.volume = volume;
    }

    private void OnLevelWasLoaded(int level)
    {
        Debug.Log("loaded level: " + level);
        if(level == SceneManager.sceneCountInBuildSettings - 1)
        {
            FadeIntoHappy();
        }
    }

    private void FadeIntoHappy()
    {
        as1.DOFade(0, fadeDuration);
        as2.DOFade(volume, fadeDuration);
    }

    private void FadeIntoPianos()
    {
        as1.DOFade(volume, fadeDuration);
        as2.DOFade(0, fadeDuration);
    }
}
