using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    private bool isAnimating = false;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        DontDestroyOnLoad(this);
    }

    private void Update()
    {
        if (!isAnimating)
        {
            Player player = GameObject.FindObjectOfType<Player>();

            if (player != null)
                this.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, this.transform.position.z);
        }
    }

    public void GoTo(Vector3 p, float d = 0.3f)
    {
        isAnimating = true;
        Vector3 animationTarget = new Vector3(p.x, p.y, this.transform.position.z);
        this.transform.DOMove(animationTarget, d).OnComplete(() => {
            isAnimating = false;
        });
    }
}
