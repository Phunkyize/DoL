using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSwitchOff : MonoBehaviour
{
    public GameObject Button;
    public Canvas canvas;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Button.SetActive(false);
            canvas.gameObject.SetActive(false);
            Destroy(this.transform.parent.gameObject);
        }
    }
}
