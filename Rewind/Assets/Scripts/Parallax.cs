using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float length, startPos;

    public Camera cam;
    public float pFactor;

    public bool shouldParallaxInY;

    private float cachedY;

    void Start()
    {
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
        cachedY = this.transform.position.y;
    }

    void Update()
    {
        float temp = cam.transform.position.x * (1 - pFactor);
        float dist = cam.transform.position.x * pFactor;

        if(shouldParallaxInY)
            transform.position = new Vector3(startPos + dist, transform.position.y, transform.position.z);
        else
            transform.position = new Vector3(startPos + dist, cachedY, transform.position.z);

        if (temp > startPos + length) startPos += length;
        else if (temp < startPos - length) startPos -= length;
    }
}
