using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class SoundtrackManager : MonoBehaviour
{
    public static SoundtrackManager instance;

    [SerializeField]
    private AudioSource as1;
    [SerializeField]
    private AudioSource as2;

    public float fadeDuration;
    public float volume;

    private void Awake()
    {
        if (instance == null)
            instance = this;
            
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        as1.volume = volume;
        as2.volume = volume;
    }

    private void OnLevelWasLoaded(int level)
    {
        if(level == SceneManager.sceneCountInBuildSettings - 1)
        {
            FadeIntoHappy();
        }
    }

    private void FadeIntoHappy()
    {
        as2.Play();
        as1.DOFade(0, fadeDuration).OnComplete(() => as1.Stop());
        as2.DOFade(volume, fadeDuration);
    }

    private void FadeIntoPianos()
    {
        as1.Play();
        as1.DOFade(volume, fadeDuration);
        as2.DOFade(0, fadeDuration).OnComplete(() => as2.Stop());
    }

    public void PlayOneShot(AudioClip clip, float volMult = 1)
    {
        as2.PlayOneShot(clip, volMult);
    }
}
