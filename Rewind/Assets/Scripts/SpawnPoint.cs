using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    void Awake()
    {
        if (GameObject.FindGameObjectWithTag("Player") == null)
        {
            SpawnPlayer();
        }
    }

    public void SpawnPlayer()
    {
        Vector3 spawnPos = new Vector3(this.transform.position.x, this.transform.position.y, 0);
        GameObject playerGO = Instantiate(player);
        playerGO.transform.position = spawnPos;
    }
}
