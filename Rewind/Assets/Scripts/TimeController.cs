using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    public GameObject player;

    public ArrayList playerPositions;

    void Start()
    {
        playerPositions = new ArrayList();
    }

    void FixedUpdate()
    {
        playerPositions.Add(player.transform.position);
    }

    public void SpawnPlayerAndReverse()
    {
        StartCoroutine(ReverseCoroutine());
    }

    private IEnumerator ReverseCoroutine()
    {
        ArrayList positions = this.playerPositions;
        GameObject playerClone = Instantiate(player);
        playerClone.GetComponent<Player>().enabled = false;

        for (var i = positions.Count - 1; i > 0; i--)
        {
            playerClone.transform.position = (Vector3)positions[i];
            yield return new WaitForEndOfFrame();
        }
    }
}
