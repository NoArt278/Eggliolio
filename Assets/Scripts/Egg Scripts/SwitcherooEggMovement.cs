using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitcherooEggMovement : PoweredEgg
{
    GameObject arrow;
    private int moveSign, switchInterval, selectedColor;
    private float prevSwitchTime;
    private bool isBlinking;
    private Color[] colors;
    // Start is called before the first frame update
    private void Start()
    {
        arrow = transform.GetChild(0).gameObject;
        moveSign = -1;
        colors = new Color[2];
        colors[0] = sr.color; // Right color
        colors[1] = Color.red; // Left color
        selectedColor = 0;
        switchInterval = Random.Range(5, 11);
        prevSwitchTime = Time.time;
        isBlinking = false;
    }

    private void Update()
    {
        if (switchInterval - (Time.time -  prevSwitchTime) <= 3)
        {
            if (!isBlinking)
            {
                StartCoroutine(Blink());
            }
        }
    }

    public override void MoveEgg()
    {
        rb.AddTorque(Input.GetAxis("Horizontal") * speed * moveSign);
        if (notFlat)
        {
            rb.AddForce(new Vector2(3, 3 * floorAngleSign) * Input.GetAxis("Horizontal") * moveSign * -1);
        }
        if (!isGrounded)
        {
            rb.AddForce(new Vector2(speed, 0) * Input.GetAxis("Horizontal") * moveSign * -1);
        }
    }

    IEnumerator Blink()
    {
        isBlinking = true;
        while (Time.time-prevSwitchTime < switchInterval)
        {
            selectedColor++;
            selectedColor %= colors.Length;
            sr.color = colors[selectedColor];
            yield return new WaitForSeconds(0.1f);
            selectedColor++;
            selectedColor %= colors.Length;
            sr.color = colors[selectedColor];
            yield return new WaitForSeconds(0.5f);
        }
        selectedColor++;
        selectedColor %= colors.Length;
        sr.color = colors[selectedColor];
        arrow.transform.Rotate(new Vector3(0, 180, 0));
        moveSign *= -1;
        prevSwitchTime = Time.time;
        switchInterval = Random.Range(5, 11);
        isBlinking = false;
    }
}
