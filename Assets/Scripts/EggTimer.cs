using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggTimer : MonoBehaviour
{
    public float currTime;
    public bool stopTimer;
    private float prevTime;
    // Start is called before the first frame update
    void Start()
    {
        prevTime = Time.time;
        stopTimer = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!stopTimer)
            currTime = Time.time - prevTime;
    }
}
