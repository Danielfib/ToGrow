using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainScreenManager : MonoBehaviour
{
    public GameObject mainScreenPanel;
    public GameObject tutorialScreenPanel;
    public GameObject creditsPanel;

    public void GoToTutorialScreen()
    {
        mainScreenPanel.SetActive(false);
        tutorialScreenPanel.SetActive(true);
        creditsPanel.SetActive(false);
    }

    public void GoToMainScreen()
    {
        mainScreenPanel.SetActive(true);
        tutorialScreenPanel.SetActive(false);
        creditsPanel.SetActive(false);
    }

    public void GoToCredits()
    {
        mainScreenPanel.SetActive(false);
        tutorialScreenPanel.SetActive(false);
        creditsPanel.SetActive(true);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
}
