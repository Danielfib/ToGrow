using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class SpawnPoint : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    [SerializeField]
    private Material normal;
    [SerializeField]
    private Material greyscale;

    [SerializeField]
    private AudioClip activatedSound;

    private SpriteRenderer sr;
    private UnityEngine.Rendering.Universal.Light2D light2D;
    private Flickering2DLight flickering2DLight;

    private bool isActive = false;

    void Awake()
    {
        if (GameObject.FindGameObjectWithTag("Player") == null && this.name == "InitialCheckpoint")
        {
            SpawnPlayer();
        }

        sr = GetComponent<SpriteRenderer>();
        light2D = GetComponent<UnityEngine.Rendering.Universal.Light2D>();
        flickering2DLight = GetComponent<Flickering2DLight>();
    }

    public void SpawnPlayer()
    {
        Vector3 spawnPos = new Vector3(this.transform.position.x, this.transform.position.y, 0);
        CameraController.instance.GoTo(spawnPos);
        GameObject playerGO = Instantiate(player);
        playerGO.transform.position = spawnPos;
        playerGO.GetComponent<Player>().lastCheckpoint = this;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player"
           && !isActive)
        {
            Player player = collision.GetComponent<Player>();
            player.lastCheckpoint = this;
            ActivateCheckpoint();

            var otherCheckpoints = GameObject.FindObjectsOfType<SpawnPoint>().Where(x => x != this);
            foreach(var otherCheckpoint in otherCheckpoints)
            {
                otherCheckpoint.DeactivateCheckpoint();
            }
        }
    }

    public void ActivateCheckpoint()
    {
        SoundtrackManager.instance?.PlayOneShot(activatedSound);
        sr.material = normal;
        flickering2DLight.enabled = true;
        light2D.enabled = true;
        isActive = true;
    }

    public void DeactivateCheckpoint()
    {
        sr.material = greyscale;
        flickering2DLight.enabled = false;
        light2D.enabled = false;
        isActive = false;
    }
}
