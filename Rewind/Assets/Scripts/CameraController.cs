﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.Cinemachine;

public class CameraController : MonoBehaviour
{
    [SerializeField] 
    private CinemachineCamera cinemachineCamera;
    
    private bool isAnimating = false;

    private void Update()
    {
        if (!isAnimating)
        {
            Player player = FindFirstObjectByType<Player>();

            if (player != null)
                this.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, this.transform.position.z);
        }
    }

    private void GoTo(Vector3 p, float d = 0.3f)
    {
        isAnimating = true;
        Vector3 animationTarget = new Vector3(p.x, p.y, this.transform.position.z);
        this.transform.DOMove(animationTarget, d).OnComplete(() => {
            isAnimating = false;
        });
    }

    public void FollowPlayer(Transform playerTransform)
    {
        //GoTo(playerTransform.position);
        cinemachineCamera.Follow = playerTransform;
    }
}
