using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScreenEgg : MonoBehaviour
{
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(rb.angularVelocity) < 200)
        {
            rb.AddTorque(-5);
        }
    }
}
