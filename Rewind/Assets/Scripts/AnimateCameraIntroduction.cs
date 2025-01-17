using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateCameraIntroduction : MonoBehaviour
{
    [SerializeField] private Transform cam;
    [SerializeField] private float speed;
    
    void Update()
    {
        cam.transform.position += Vector3.right * Time.deltaTime * speed;
    }
}
