using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sense : MonoBehaviour
{
    public bool bDebug = true;
    public float detectionRate = 1.0f;

    protected float elapsedTime = 0.0f;

    protected virtual void Initialise() { }
    protected virtual void UpdateSense() { }
    // Start is called before the first frame update
    void Start()
    {
        elapsedTime = 0.0f;
        Initialise();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSense();
    }
}
