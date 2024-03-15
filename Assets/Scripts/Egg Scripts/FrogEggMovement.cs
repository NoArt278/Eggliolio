using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogEggMovement : PoweredEgg
{
    [SerializeField] int jumpSpeed;
    public override void MoveEgg()
    {
        base.MoveEgg();
        if (isGrounded)
        {
            rb.AddForce(new Vector2(Input.GetAxis("Horizontal"), 1) * Input.GetAxis("Jump") * jumpSpeed, ForceMode2D.Impulse);
        }
        if (!isGrounded)
        {
            rb.AddForce(new Vector2(speed, 0) * Input.GetAxis("Horizontal"));
        }
    }
}
