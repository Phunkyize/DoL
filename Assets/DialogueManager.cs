using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;
    public Queue<DialogueLine> lines;

    public Button PlayerDialogOption1;
    public Button PlayerDialogOption2;
    public Button LightDialogOption1;
    public Button LightDialogOption2;
    private Button OtherDialogOption;

    private TMP_Text PlayerDialogText1;
    private TMP_Text PlayerDialogText2;
    private TMP_Text LightDialogText1;
    private TMP_Text LightDialogText2;
    private TMP_Text OtherDialogText;

    private void Awake()
    {
        if (instance != null)
        {
            if (instance != this)
            {
                Destroy(this.gameObject);
            }
            Debug.LogError("More then one DialogueManager in scene");
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        lines = new Queue<DialogueLine>();
        PlayerDialogText1= PlayerDialogOption1.transform.GetChild(0).GetComponent<TMP_Text>();
        PlayerDialogText2 = PlayerDialogOption2.transform.GetChild(0).GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartDialogue(DialogueLine[] dialogue, Button OtherDialogOption)
    {
        GameMaster.EnablePlayerMovement(false);
        lines.Clear();
        foreach(DialogueLine line in dialogue)
        {
            lines.Enqueue(line);
        }
        this.OtherDialogOption = OtherDialogOption;
        this.OtherDialogText = OtherDialogOption.transform.GetChild(0).GetComponent<TMP_Text>();
        DialogueLine firstLine = lines.Peek();
        switch (firstLine.speaker) {
            case (Speaker.Player):
                EventSystem.current.SetSelectedGameObject(PlayerDialogOption1.gameObject);
                break;
            case (Speaker.Light):
                EventSystem.current.SetSelectedGameObject(LightDialogOption1.gameObject);
                break;
            case (Speaker.Other):
                EventSystem.current.SetSelectedGameObject(OtherDialogOption.gameObject);
                break;
        }
        DisplayNextLine();
        

    }

    public void DisplayNextLine()
    {
        if (lines.Count == 0)
        {
            EndDialog();
            return;
        }
        DialogueLine dialogueLine = lines.Dequeue();
        switch (dialogueLine.speaker)
        {
            case (Speaker.Player):
                if (dialogueLine.choice)
                {
                    PlayerDialogOption1.gameObject.SetActive(true);
                    PlayerDialogText1.text = dialogueLine.line;
                    PlayerDialogOption2.gameObject.SetActive(true);
                    dialogueLine = lines.Dequeue();
                    PlayerDialogText2.text = dialogueLine.line;
                    EventSystem.current.SetSelectedGameObject(PlayerDialogOption1.gameObject);
                }
                break;
            case (Speaker.Light):
                EventSystem.current.SetSelectedGameObject(LightDialogOption1.gameObject);
                break;
            case (Speaker.Other):
                OtherDialogOption.gameObject.SetActive(true);
                OtherDialogText.text = dialogueLine.line;
                EventSystem.current.SetSelectedGameObject(OtherDialogOption.gameObject);
                break;
        }
    }
    public void EndDialog()
    {
        
        GameMaster.EnablePlayerMovement(true);
        PlayerDialogOption1.gameObject.SetActive(false);
        PlayerDialogOption2.gameObject.SetActive(false);
        OtherDialogOption.gameObject.SetActive(false);
    }


   
}
