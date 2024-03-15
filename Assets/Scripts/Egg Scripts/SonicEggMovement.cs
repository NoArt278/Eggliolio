using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonicEggMovement : PoweredEgg
{
    public override void MoveEgg()
    {
        base.MoveEgg();
        rb.AddForce(new Vector2(speed, 0) * Input.GetAxis("Horizontal"));
    }
}
