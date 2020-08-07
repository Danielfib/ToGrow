using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    [SerializeField]
    private Material normal;
    [SerializeField]
    private Material greyscale;

    private SpriteRenderer sr;
    private Light2D light2D;
    private Flickering2DLight flickering2DLight;

    void Start()
    {
        if (GameObject.FindGameObjectWithTag("Player") == null && this.name == "InitialCheckpoint")
        {
            SpawnPlayer();
        }

        sr = GetComponent<SpriteRenderer>();
        light2D = GetComponent<Light2D>();
        flickering2DLight = GetComponent<Flickering2DLight>();
    }

    public void SpawnPlayer()
    {
        Vector3 spawnPos = new Vector3(this.transform.position.x, this.transform.position.y, 0);
        CameraController.instance.GoTo(spawnPos);
        GameObject playerGO = Instantiate(player);
        playerGO.transform.position = spawnPos;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
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
        sr.material = normal;
        flickering2DLight.enabled = true;
        light2D.enabled = true;
    }

    public void DeactivateCheckpoint()
    {
        sr.material = greyscale;
        flickering2DLight.enabled = false;
        light2D.enabled = false;
    }
}
