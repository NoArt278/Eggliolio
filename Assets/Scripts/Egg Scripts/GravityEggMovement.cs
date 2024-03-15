using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityEggMovement : PoweredEgg
{
    public override void MoveEgg()
    {
        base.MoveEgg();
        rb.AddForce(new Vector2(speed + 2, 0) * Input.GetAxis("Horizontal"));
    }
}
