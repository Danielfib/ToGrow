using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textDisplay;
    public string[] sentences;
    private int index;
    private float typeSpeed = 0.02f;

    public GameObject dialogueCanvas;
    public GameObject continueButton;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if(sentences.Length > 0)
        {
            GameManager.instance?.DisablePlayerInput();
            FadeInDialoguePanel();
        } else
        {
            GameManager.instance?.EnablePlayerInput();
            dialogueCanvas.SetActive(false);
            this.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            UserSkip();
        }
    }

    private void UserSkip()
    {
        if (continueButton.activeInHierarchy)
        {
            NextSentence();
        } else
        {
            StopAllCoroutines();
            //StopCoroutine(Type());
            textDisplay.text = sentences[index];
            continueButton.SetActive(true);
        }
    }

    IEnumerator Type()
    {
        continueButton.SetActive(false);
        textDisplay.text = "";
        foreach (char letter in sentences[index].ToCharArray())
        {
            textDisplay.text += letter;
            yield return new WaitForSeconds(typeSpeed);
        }

        continueButton.SetActive(true);
    }

    public void NextSentence()
    {
        audioSource?.Play();
        if(index < sentences.Length - 1)
        {
            index++;
            textDisplay.text = "";
            StartCoroutine(Type());
        } else
        {
            //dialogue complete
            //textDisplay.text = "";
            FadeOutDialoguePanel();
        }
    }

    private void FadeOutDialoguePanel()
    {
        GameManager.instance?.FinishedInitialDialogue();
        dialogueCanvas.GetComponent<CanvasGroup>().DOFade(0, 1).OnComplete(() => {
            dialogueCanvas.SetActive(false);
            this.gameObject.SetActive(false);
        });
    }

    private void FadeInDialoguePanel()
    {
        dialogueCanvas.SetActive(true);
        dialogueCanvas.GetComponent<CanvasGroup>().alpha = 0;
        dialogueCanvas.GetComponent<CanvasGroup>().DOFade(1, 1).OnComplete(() => StartCoroutine(Type()));
    }
}
