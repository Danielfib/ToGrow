using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    public GameObject playerClonePrefab;
    public ArrayList playerPositions;
    private GameObject player;

    public static TimeController instance;

    [SerializeField]
    [Tooltip("How many frames between clone position update")]
    private int speedRatio = 3; // frames to skip per clone update

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Start()
    {
        playerPositions = new ArrayList();
        TryGetPlayer();
    }

    void FixedUpdate()
    {
        if (player == null)
            TryGetPlayer();


        //prevent clone to stay still when player stood still (should I prevent this?)
        if(playerPositions.Count > 0)
        {
            if(player.transform.position != (Vector3)playerPositions[Mathf.Max(playerPositions.Count-1, 0)])
            {
                playerPositions.Add(player.transform.position);
            }
        } else
        {
            playerPositions.Add(player.transform.position);
        }
    }

    public void SpawnPlayerAndReverse()
    {
        StartCoroutine(ReverseCoroutine((ArrayList)this.playerPositions.Clone()));
        this.playerPositions.Clear();
    }

    private IEnumerator ReverseCoroutine(ArrayList positions)
    {
        GameObject playerClone = Instantiate(playerClonePrefab);

        for (var i = positions.Count - 1; i > 0; i--)
        {

            playerClone.transform.position = (Vector3)positions[i];

            for (var s = 0; s < this.speedRatio; s++)
                yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(1);
        Destroy(playerClone);
    }

    private void TryGetPlayer()
    {
        this.player = GameObject.FindGameObjectWithTag("Player");

        if (this.player == null)
        {
            Debug.LogError("did not find player in scene!");
        }
    }
}
