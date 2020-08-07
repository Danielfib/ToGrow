using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField]
    private Scene[] levels;

    private int currentLvl = 0;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        DontDestroyOnLoad(this);
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
        EnablePlayerInput();
    }

    public void TransitionToNextScene()
    {
        currentLvl++;
        if (currentLvl == levels.Length)
        {
            //TODO: game finish screen
        } else
        {
            SceneManager.LoadScene(levels[currentLvl].handle);
        }
    }
}
