using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Cinemachine;
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
    
    [SerializeField] private float coyoteTimeDuration;
    private float coyoteTimer;
    
    [Header("Jump")]
    [SerializeField]
    private float initialJumpForce = 80f;
    [SerializeField]
    private float holdForce = 1000f;
    [SerializeField]
    private float maxHoldTime = 0.3f;
    
    private bool isJumping;
    private float jumpHoldTimer;
    
    private bool isDying;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        GetComponentInChildren<TrailRenderer>().time = TimeController.instance.recordTime;
    }

    void Update()
    {
        if (isDying)
        {
            return;
        }
        
        var newIsOnGround = IsOnGround();
        if (!isOnGround && newIsOnGround)
        {
            //just landed
            coyoteTimer = coyoteTimeDuration;
            spriteHolder.DOComplete();
            spriteHolder.DOPunchScale(new Vector3(1f, -.6f, 1), .3f, 0, 1);
        }
        
        isOnGround = newIsOnGround;
        if (!isOnGround)
        {
            coyoteTimer -= Time.deltaTime;
        }
        animator.SetBool("IsGrounded", isOnGround);

        this.rb.linearVelocity = new Vector2(Input.GetAxis("Horizontal") * 5, rb.linearVelocity.y);
        FlipSpriteOnWalkDirection();



        if (Input.GetKeyDown(KeyCode.Space))
        {
            if ((coyoteTimer > 0 || isOnGround || this.transform.parent != null)
                && !isJumping)
            {
                Jump();
            } 
        }
        
        if (Input.GetKey(KeyCode.Space))
        {
            if (isJumping)
            {
                ProcessJumping();
            }
        }

        if (Input.GetKeyUp(KeyCode.Space) || jumpHoldTimer >= maxHoldTime)
        {
            isJumping = false;
        }
    }

    private void ProcessJumping()
    {
        if (jumpHoldTimer < maxHoldTime)
        {
            rb.AddForce(Vector3.up * (holdForce * Time.deltaTime));
            jumpHoldTimer += Time.deltaTime;
        }
    }

    private void LateUpdate()
    {
        if (this.transform.parent == null
            && !isDying)
        {
            if (rb.linearVelocity.y <= 0)
            {
                rb.gravityScale = 3;
            } else if (rb.linearVelocity.y > 0)
            {
                rb.gravityScale = 1;
            }
        }
        else
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(0, rb.linearVelocity.y));
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
        isJumping = true;
        coyoteTimer = 0;
        jumpHoldTimer = 0;
        
        SoundtrackManager.instance?.PlayOneShot(jumpSound, 4);
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(Vector3.up * initialJumpForce);
        spriteHolder.DOComplete();
        spriteHolder.DOPunchScale(new Vector3(-.6f, 1f, 0f), .3f, 0, 1);

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
        rb.linearVelocity = Vector2.zero;
        isDying = true;
        spriteHolder.DOComplete();
        spriteHolder.DOScale(Vector3.one * .4f, .2f).SetEase(Ease.OutBounce).OnComplete(OnDieAnimationFinish);
    }

    private void OnDieAnimationFinish()
    {
        if (lastCheckpoint == null)
            lastCheckpoint = GameObject.Find("InitialCheckpoint").GetComponent<SpawnPoint>();

        SoundtrackManager.instance?.PlayOneShot(dieSound, 4);
        TimeController.instance.SpawnPlayerAndReverse();
        lastCheckpoint.SpawnPlayer();
        Destroy(this.gameObject);
    }

    public void AnimateLatch(Vector3 transformPosition)
    {
        var normalizedDir = (transformPosition - transform.position).normalized;
        spriteHolder.DOComplete();
        spriteHolder.DOPunchScale(normalizedDir, .4f, 0, 1);
    }
}
