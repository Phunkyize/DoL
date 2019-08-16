using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueTrigger : MonoBehaviour
{
    public Button OtherDialogOption;
  
    public DialogueLine[] dialogue;
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            DialogueManager.instance.StartDialogue(dialogue, OtherDialogOption);
            Destroy(this.gameObject);
        }
    }

    
}

