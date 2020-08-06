using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

[RequireComponent(typeof(Light2D))]
public class Flickering2DLight : MonoBehaviour
{
    private new Light2D light;
    private float randomizer;
    private float backupOuterRadius;

    // Start is called before the first frame update
    void Start()
    {
        light = GetComponent<Light2D>();
        backupOuterRadius = light.pointLightOuterRadius;
    }

    // Update is called once per frame
    void Update()
    {
        randomizer = Random.value;
        if(randomizer > 0.98)
        {
            float r = Random.Range(0.95f, 1.05f);
            light.intensity = r;
            light.pointLightOuterRadius = backupOuterRadius * r;
        }
    }
}
