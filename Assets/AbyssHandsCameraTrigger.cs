using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbyssHandsCameraTrigger : MonoBehaviour
{
    public Cinemachine.CinemachineVirtualCamera vcam2;
    public ChasingHands abyssHands;
    public DialogueScript script;
    private bool triggered=false;

    private void Update()
    {

        if (triggered)
        {
            if (!DialogueManager.instance.dialogueInProgress)
            {
                GameMaster.SetCameraAbyssChase(vcam2);
                abyssHands.Activate(15f, 20f);
                Destroy(this.gameObject);
            }
        }
    
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        triggered = true;
        if (collision.CompareTag("Player"))
        {
            DialogueManager.instance.setDialogueScript(script);
            DialogueManager.instance.StartDialogue();
        }
       
       
    }
}
