using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    void Update()
    {
        transform.Translate(Vector3.forward * 8.0f * Time.deltaTime * Input.GetAxis("Vertical"));
        transform.Rotate(Vector3.up * 200.0f * Time.deltaTime * Input.GetAxis("Horizontal"));
    }
}
