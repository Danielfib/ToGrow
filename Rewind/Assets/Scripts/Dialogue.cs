using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textDisplay;
    public string[] sentences;
    private int index;
    public float typeSpeed;

    public GameObject continueButton;

    private AudioSource audioSource;

    public void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(Type());
    }

    IEnumerator Type()
    {
        continueButton.SetActive(false);

        foreach(char letter in sentences[index].ToCharArray())
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
            continueButton.SetActive(false);
            textDisplay.text = "";
        }
    }
}
