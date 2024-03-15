using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundDarken : MonoBehaviour
{
    private void Start()
    {
        GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width * 3, Screen.height * 3);
    }
}
