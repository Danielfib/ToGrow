using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(UnityEngine.Rendering.Universal.Light2D))]
public class Flickering2DLight : MonoBehaviour
{
    private new UnityEngine.Rendering.Universal.Light2D light;
    private float randomizer;
    private float backupOuterRadius;

    // Start is called before the first frame update
    void Start()
    {
        light = GetComponent<UnityEngine.Rendering.Universal.Light2D>();
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
