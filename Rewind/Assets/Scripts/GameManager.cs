using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private int currentLvl = 1;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        DontDestroyOnLoad(this);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseMenuManager.instance.OpenPauseMenu();
        }
    }

    public void DisablePlayerInput()
    {
        var playerGO = GameObject.FindGameObjectWithTag("Player");
        if (playerGO != null)
        {
            playerGO.GetComponent<Player>().enabled = false;
        }
    }

    public void EnablePlayerInput()
    {
        var playerGO = GameObject.FindGameObjectWithTag("Player");
        if (playerGO != null)
        {
            playerGO.GetComponent<Player>().enabled = true;
        } else
        {
            TransitionToNextScene();
        }
    }

    public void FinishedInitialDialogue()
    {
        if (currentLvl == SceneManager.sceneCountInBuildSettings - 1)
        {
            //ending scene
            FindFirstObjectByType<Camera>().transform.position = new Vector3(0, 0, -10);
            FindFirstObjectByType<CanvasGroup>().DOFade(0, 1);
        }
        else
        {
            EnablePlayerInput();
        }
    }

    public void TransitionToNextScene()
    {
        currentLvl++;
        if (currentLvl < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(currentLvl);
        }
    }
}
