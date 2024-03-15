using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float length, height, startPosX, startPosY;
    public GameObject cam;
    public float parallaxEffect;
    void Start()
    {
        startPosX = transform.position.x;
        startPosY = transform.position.y;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
        height = GetComponent<SpriteRenderer>().bounds.size.y;
    }

    void Update()
    {
        float tempX = cam.transform.position.x * (1 - parallaxEffect);
        float distX = cam.transform.position.x * parallaxEffect;
        float tempY = cam.transform.position.y * (1 - parallaxEffect);
        float distY = cam.transform.position.y * parallaxEffect;

        transform.position = new Vector3(startPosX + distX, startPosY + distY, transform.position.z);
        if (tempX > startPosX + length) startPosX += length;
        else if (tempX < startPosX - length) startPosX -= length;
        if (tempY > startPosY + height) startPosY += height;
        else if (tempY < startPosY - height) startPosY -= height;
    }
}
