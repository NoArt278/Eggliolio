using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggCam : MonoBehaviour
{
    public GameObject egg, stove;
    private float minPos, maxPos, minYPos, verticalSpeed;
    private EggScript eggScript;
    private void Awake()
    {
        minPos = 0;
        maxPos = stove.transform.position.x - 10;
        minYPos = 0;
        eggScript = egg.GetComponent<EggScript>();
        verticalSpeed = 2f;
    }
    private void Update()
    {
        float heightDiff = egg.transform.position.y - transform.position.y;
        float camY = transform.position.y;
        if (Mathf.Abs(heightDiff) >= 10f)
        {
            if (!eggScript.isGrounded || eggScript.notFlat)
            {
                camY = egg.transform.position.y - 10f * Mathf.Sign(heightDiff);
            }
            else
            {
                camY = Mathf.Lerp(camY, egg.transform.position.y, Time.deltaTime * verticalSpeed);
            }
        }
        if (Mathf.Abs(heightDiff) >= 3f && !eggScript.zoomedOut)
        {
            if (!eggScript.isGrounded || eggScript.notFlat)
            {
                camY = egg.transform.position.y - 3f * Mathf.Sign(heightDiff);
            } else
            {
                camY = Mathf.Lerp(camY, egg.transform.position.y, Time.deltaTime * verticalSpeed);
            }
        }
        camY = Mathf.Clamp(camY, minYPos, Mathf.Infinity);
        transform.position = new Vector3(Mathf.Clamp(egg.transform.position.x+3, minPos, maxPos), camY, transform.position.z);
    }

    public void ChangeEgg(GameObject newEgg) {
        egg = newEgg;
        eggScript = newEgg.GetComponent<EggScript>();
    }
}
