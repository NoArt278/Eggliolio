using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceEggMovement : PoweredEgg
{
    public override void MoveEgg()
    {
        rb.AddForce(new Vector2(1,0) * Input.GetAxis("Horizontal") * speed);
    }
}
