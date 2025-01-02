﻿using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [SerializeField] 
    private Transform spriteHolder;
    
    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    private LayerMask whatIsGround;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private AudioClip jumpSound;
    [SerializeField]
    private AudioClip dieSound;

    private Rigidbody2D rb;

    [HideInInspector]
    public SpawnPoint lastCheckpoint;

    [HideInInspector]
    public bool isAvoidingClones;

    private bool isOnGround;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        GetComponent<TrailRenderer>().time = TimeController.instance.recordTime;
    }

    void Update()
    {
        var newIsOnGround = IsOnGround();
        if (!isOnGround && newIsOnGround)
        {
            //just landed
            spriteHolder.DOPunchScale(new Vector3(1.2f, -.6f, 1), .4f, 0, 1);
        }
        isOnGround = newIsOnGround;
        animator.SetBool("IsGrounded", isOnGround);

        this.rb.linearVelocity = new Vector2(Input.GetAxis("Horizontal") * 5, rb.linearVelocity.y);
        FlipSpriteOnWalkDirection();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isOnGround || this.transform.parent != null)
            {
                Jump();
            }
        }
    }

    private void LateUpdate()
    {
        if (this.transform.parent == null)
        {
            if (rb.linearVelocity.y < 0)
            {
                rb.gravityScale = 3;
            } else if (rb.linearVelocity.y > 0)
            {
                rb.gravityScale = 1;
            }
        }
        else
        {
            rb.gravityScale = 0;
        }
    }

    private void FlipSpriteOnWalkDirection()
    {
        float currentVelX = this.gameObject.GetComponent<Rigidbody2D>().linearVelocity.x;
        if (currentVelX < 0)
            transform.eulerAngles = new Vector3(0, 180, 0);
        else if (currentVelX > 0)
            transform.eulerAngles = new Vector3(0, 0, 0);
    }

    void Jump()
    {
        SoundtrackManager.instance?.PlayOneShot(jumpSound, 4);
        this.GetComponent<Rigidbody2D>().AddForce(Vector3.up * 380);
        spriteHolder.DOPunchScale(new Vector3(-.6f, 1.2f, 0f), .4f, 0, 1);

        if (transform.parent != null)
            StartCoroutine(AvoidCloneCoroutine());
    }

    private IEnumerator AvoidCloneCoroutine()
    {
        isAvoidingClones = true;
        yield return new WaitForSeconds(0.3f);
        isAvoidingClones = false;
    }

    private bool IsOnGround()
    {
        Collider2D[] colliding = Physics2D.OverlapCircleAll(this.groundCheck.position, 0.2f);

        foreach(var col in colliding)
        {
            if(whatIsGround == (whatIsGround | (1 << col.gameObject.layer)))
            {
                return true;
            }
        }

        return false;
    }

    public void Die()
    {
        if (lastCheckpoint == null)
            lastCheckpoint = GameObject.Find("InitialCheckpoint").GetComponent<SpawnPoint>();

        SoundtrackManager.instance?.PlayOneShot(dieSound, 4);
        TimeController.instance.SpawnPlayerAndReverse();
        lastCheckpoint.SpawnPlayer();
        Destroy(this.gameObject);
    }
}
