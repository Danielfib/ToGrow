using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainScreenManager : MonoBehaviour
{
    public GameObject mainScreenPanel;
    public GameObject tutorialScreenPanel;

    public void GoToTutorialScreen()
    {
        mainScreenPanel.SetActive(false);
        tutorialScreenPanel.SetActive(true);
    }

    public void GoToMainScreen()
    {
        mainScreenPanel.SetActive(true);
        tutorialScreenPanel.SetActive(false);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
}
