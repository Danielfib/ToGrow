using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    private LayerMask whatIsGround;


    void Update()
    {
        transform.Translate(Vector3.right * 8.0f * Time.deltaTime * Input.GetAxis("Horizontal"));
        //transform.Rotate(Vector3.up * 200.0f * Time.deltaTime * Input.GetAxis("Horizontal"));

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (IsOnGround())
            {
                Jump();
            }
        }
    }

    void Jump()
    {
        this.GetComponent<Rigidbody>().AddForce(Vector3.up * 300);
    }

    private bool IsOnGround()
    {
        Collider[] colliding = Physics.OverlapSphere(this.groundCheck.position, 0.2f);

        foreach(var col in colliding)
        {
            if(whatIsGround == (whatIsGround | (1 << col.gameObject.layer)))
            {
                return true;
            }
        }

        return false;
    }
}
