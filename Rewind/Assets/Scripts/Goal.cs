using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Goal : MonoBehaviour
{
    [SerializeField]
    private AudioClip playerEnteredSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            SoundtrackManager.instance.PlayOneShot(playerEnteredSound, 4f);
            GameManager.instance.TransitionToNextScene();
        }
    }
}
