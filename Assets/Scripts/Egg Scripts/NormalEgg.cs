using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NormalEgg : EggScript
{
    [SerializeField] List<GameObject> poweredEggsList;
    private bool isBoiled = false;
    public EggCam cam;
    IEnumerator Boiling()
    {
        yield return new WaitForSeconds(2);
        int selectedPower;
        if (gm.selectedEggIndex == -1)
        {
            selectedPower = Random.Range(0, poweredEggsList.Count);
        } else
        {
            selectedPower = gm.selectedEggIndex;
        }
        GameObject powerEgg = Instantiate(poweredEggsList[selectedPower], transform.parent);
        powerEgg.transform.position = transform.position;
        cam.ChangeEgg(powerEgg);
        Destroy(gameObject);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Boiler") && !isBoiled)
        {
            StartCoroutine(Boiling());
            isBoiled = true;
        }
    }
}
