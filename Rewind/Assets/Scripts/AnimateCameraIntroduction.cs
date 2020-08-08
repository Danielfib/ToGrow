using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateCameraIntroduction : MonoBehaviour
{
    [SerializeField]
    private Transform cam;
    
    void Update()
    {
        cam.transform.position += new Vector3(0.1f, 0, 0);
    }

    private void OnDestroy()
    {
        //cam.transform.position = new Vector3(0,0,-10);
    }
}
