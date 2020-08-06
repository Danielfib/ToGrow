﻿using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PlayerClone : MonoBehaviour
{
    private float backupGravityScale = 1f;
    private LineRenderer lr;

    [SerializeField]
    private SpriteRenderer sr;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player"
            && collision.gameObject.transform.parent == null)
        {
            collision.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            //backupGravityScale = collision.GetComponent<Rigidbody2D>().gravityScale;
            collision.GetComponent<Rigidbody2D>().gravityScale = 0;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player"
            && collision.gameObject.transform.parent == null)
        {
            collision.gameObject.transform.SetParent(this.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player"
            && collision.gameObject.transform.parent == this.transform)
        {
            collision.gameObject.transform.SetParent(null);
            collision.GetComponent<Rigidbody2D>().gravityScale = backupGravityScale;
        }
    }

    private void Awake()
    {
        this.lr = this.GetComponent<LineRenderer>();
    }

    public void Reverse(Vector3[] positions, float duration)
    {
        //cut the array in half
        positions = positions.Where((x, i) => i % 2 == 0).ToArray();

        lr.positionCount = positions.Length;
        lr.SetPositions(positions);

        Color randomColor = new Color(Random.Range(0.5f, 1), Random.Range(0.5f, 1), Random.Range(0.5f, 1), 0.4f);

        sr.color = randomColor;
        lr.material.color = randomColor;

        this.transform.position = positions[0];

        //TODO: velocity should be constant. Or path length!
        this.transform.DOPath(positions,
                              duration * 2,
                              PathType.CatmullRom,
                              PathMode.Sidescroller2D,
                              5).SetEase(Ease.Flash).OnComplete(() => Destroy(this.gameObject));
    }
}
