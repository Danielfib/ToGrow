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
    
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private LayerMask whatIsClone;
    [SerializeField] private Animator animator;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip dieSound;

    private Rigidbody2D rb;

    [HideInInspector]
    public SpawnPoint lastCheckpoint;

    [HideInInspector]
    public bool isAvoidingClones;

    private bool isOnGround;
    
    [SerializeField] private float coyoteTimeDuration;
    private float coyoteTimer;
    
    [SerializeField] private float speed = 6;
    
    [Header("Jump")]
    [SerializeField] private float initialJumpForce = 200f;
    [SerializeField] private float holdForce = 1000f;
    [SerializeField] private float maxHoldTime = 0.3f;
    [SerializeField] private float gravityScaleUpwards = 3f;
    [SerializeField] private float gravityScaleDownwards = 9f;
    
    [Header("Camera Shake")]
    [SerializeField] private CinemachineImpulseSource impulseSource;
    
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
        var newIsOnClone = IsLatchedToClone();
        
        if (!isOnGround && newIsOnGround)
        {
            //just landed on ground
            coyoteTimer = coyoteTimeDuration;
            spriteHolder.DOComplete();
            spriteHolder.DOPunchScale(new Vector3(1f, -.6f, 1), .3f, 0, 1);
        }

        if (newIsOnClone)
        {
            coyoteTimer = coyoteTimeDuration;
        }
        
        isOnGround = newIsOnGround;
        if (!isOnGround && !newIsOnClone)
        {
            coyoteTimer -= Time.deltaTime;
        }
        animator.SetBool("IsGrounded", isOnGround);

        this.rb.linearVelocity = new Vector2(Input.GetAxis("Horizontal") * speed, rb.linearVelocity.y);
        FlipSpriteOnWalkDirection();



        if (Input.GetKeyDown(KeyCode.Space))
        {
            if ((coyoteTimer > 0 || isOnGround || newIsOnClone || this.transform.parent != null)
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
        if (transform.parent == null
            && !isDying)
        {
            rb.gravityScale = rb.linearVelocity.y <= 0 ? gravityScaleDownwards : gravityScaleUpwards;
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
    
    private bool IsLatchedToClone()
    {
        Collider2D[] colliding = Physics2D.OverlapCircleAll(this.groundCheck.position, 0.2f);

        foreach(var col in colliding)
        {
            if(whatIsClone == (whatIsClone | (1 << col.gameObject.layer)))
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
        SoundtrackManager.instance?.PlayOneShot(dieSound, 4);
        DoCameraShake();
        spriteHolder.DOComplete();
        spriteHolder.DOScale(Vector3.one * .4f, .3f).SetEase(Ease.OutBounce).OnComplete(OnDieAnimationFinish);
    }

    private void OnDieAnimationFinish()
    {
        if (lastCheckpoint == null)
            lastCheckpoint = GameObject.Find("InitialCheckpoint").GetComponent<SpawnPoint>();

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

    public void DoCameraShake()
    {
        impulseSource.GenerateImpulse(2f);
    }
}
