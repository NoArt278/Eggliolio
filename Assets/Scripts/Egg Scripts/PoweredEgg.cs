using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoweredEgg : EggScript
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Boiler"))
        {
            rb.velocity = new Vector2(3, 8);
        }
    }
}
