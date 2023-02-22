using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionOscillator : MonoBehaviour
{

    public Vector3 axis;
    public float amplitude;
    public float frequency;
    private Vector3 startlocal;
    private float startTime;

    public bool randomize;
    
    // Start is called before the first frame update
    void Start()
    {
        startlocal = this.transform.localPosition;
        startTime = 0;

        if (randomize)
        {
            Vector3 random = new Vector3(Random.value - 0.5f, Random.value - 0.5f, Random.value - 0.5f) * 0.4f;
            axis += random;
            amplitude += Random.value * 0.2f;
            frequency += Random.value * 0.2f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.localPosition = startlocal + axis * Mathf.Cos((startTime + Time.deltaTime) * frequency / Mathf.PI) * amplitude;
        startTime += Time.deltaTime;
    }
}
