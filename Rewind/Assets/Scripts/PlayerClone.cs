using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PlayerClone : MonoBehaviour
{
    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player"
            && collision.gameObject.transform.parent == null)
        {
            collision.gameObject.transform.SetParent(this.transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player"
            && collision.gameObject.transform.parent == this.transform)
        {
            collision.gameObject.transform.SetParent(null);
        }
    }
}
