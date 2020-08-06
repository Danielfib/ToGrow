using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    private LayerMask whatIsGround;

    [SerializeField]
    private Animator animator;

    private Rigidbody2D rb;

    private float recordMovementDuration = 3f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        animator.SetBool("IsGrounded", IsOnGround());

        this.rb.velocity = new Vector2(Input.GetAxis("Horizontal") * 5, rb.velocity.y);
        FlipSpriteOnWalkDirection();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (IsOnGround() || this.transform.parent != null)
            {
                Jump();
            }
        }
    }

    private void FlipSpriteOnWalkDirection()
    {
        float currentVelX = this.gameObject.GetComponent<Rigidbody2D>().velocity.x;
        if (currentVelX < 0)
            transform.eulerAngles = new Vector3(0, 180, 0);
        else if (currentVelX > 0)
            transform.eulerAngles = new Vector3(0, 0, 0);
    }

    void Jump()
    {
        this.GetComponent<Rigidbody2D>().AddForce(Vector3.up * 300);
    }

    private bool IsOnGround()
    {
        Collider2D[] colliding = Physics2D.OverlapCircleAll(this.groundCheck.position, 0.3f);

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
        TimeController.instance.SpawnPlayerAndReverse();
        GameObject.FindGameObjectWithTag("SpawnPoint").GetComponent<SpawnPoint>().SpawnPlayer();
        Destroy(this.gameObject);
    }
}
