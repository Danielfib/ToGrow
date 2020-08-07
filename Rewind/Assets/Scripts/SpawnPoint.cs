using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    void Start()
    {
        if (GameObject.FindGameObjectWithTag("Player") == null && this.name == "InitialCheckpoint")
        {
            SpawnPlayer();
        }
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

    }

    public void DeactivateCheckpoint()
    {

    }
}
