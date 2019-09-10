using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialSwitchOn : MonoBehaviour
{

    public GameObject Button;
    public Canvas canvas;
    private TextMeshProUGUI text;

  
    public string TutorialText;

    public Sprite buttonSprite;


   

    private void OnTriggerEnter2D(Collider2D collision)
    {
        text = canvas.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        text.text = TutorialText;
        
                Button.GetComponent<SpriteRenderer>().sprite = buttonSprite;
            

        Debug.Log("here");
            Button.SetActive(true);
            canvas.gameObject.SetActive(true);
        
    }
}



