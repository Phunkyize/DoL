using UnityEngine;
using UnityEngine.UI;
using Dialogue;
using Dialogue.Nodes;
using UnityEngine.EventSystems;
using TMPro;

public class DialogueManager : MonoBehaviour
{

    public static DialogueManager instance;
    [SerializeField] private DialogueScript dialogueScript = null;
    

    public Button button1;
    public Button button2;
    public Button button3;
    private TextMeshProUGUI text1;
    private TextMeshProUGUI text2;
    private TextMeshProUGUI text3;

    public Button buttonLight;
    private TextMeshProUGUI textLight;
    public Button buttonOther;
    private TextMeshProUGUI textOther;

    public bool dialogueInProgress;
    private bool selfTalk;
    DialogueGraph graph;

    private void Awake()
    {
        if (instance != null)
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }
            Debug.LogError("More then one DialogueManager in scene");
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    
    }

        // Start is called before the first frame update
        void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Next()
    {
        
        DialogueNode node = graph.GetCurrent();
        if (node is StartNode startNode)
        {
            Continue();
        }
        else if (node is TextOtherNode textOtherNode)
        {
            OtherSpeaking();
            textOther.text = textOtherNode.GetText();
          
        }

        else if (node is TextLightNode textLightNode)
        {
            LightSpeaking();
            textLight.text = textLightNode.GetText();
          
        }

        else if (node is ChoiceNode choiceNode)
        {
            text1.text = choiceNode.GetChoiceText(0);
            text2.text = choiceNode.GetChoiceText(1);
            string choice3 = choiceNode.GetChoiceText(2);

            if (choice3!= "")
            {
                text3.text = choice3;

                PlayerSpeaking(3);
            }
            PlayerSpeaking(2);
            

        }
        else if (node is EventNode eventNode)
        {
            
            if (eventNode.trigger != null)
            {
                eventNode.Trigger();
            }
            else
            {
                Debug.Log("No event was set");
            }
            Continue();

           
        }

        else if (node is ExitNode exitNode)
        {
            FinishDialogue();

            Debug.Log("Exit");
            
        }
    }

    public void MakeAChoice(int choice)
    {
        DialogueNode node = graph.GetCurrent();
        if (node is ChoiceNode)
        {
            ChoiceNode dialogueChoiceNode = (ChoiceNode)node;
            dialogueChoiceNode.SetChoice(choice);
        }
        Continue();
    }

    public void Continue()
    {
        graph.Continue();
        Next();
    }

    public void StartDialogue(Button otherBubble)
    {
        selfTalk = false;
        dialogueInProgress = true;
        GameMaster.EnablePlayerMovement(false);
        graph = dialogueScript.GetDialogueGraph();
        graph.Initialize();
        buttonOther = otherBubble;
       
        text1 = button1.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        text2 = button2.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        text3 = button3.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        textOther = buttonOther.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        textLight = buttonLight.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        Next();
    }

    public void StartDialogue()
    {
        selfTalk = true;
        dialogueInProgress = true;
        GameMaster.EnablePlayerMovement(false);
        graph = dialogueScript.GetDialogueGraph();
        graph.Initialize();
        text1 = button1.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        text2 = button2.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        text3 = button3.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        textLight = buttonLight.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        Next();
    }


    private void OtherSpeaking()
    {
        buttonOther.gameObject.SetActive(true);
        buttonOther.interactable = true;
        buttonLight.interactable = false;
        button1.gameObject.SetActive(false);
        button2.gameObject.SetActive(false);
        button3.gameObject.SetActive(false);
        EventSystem.current.SetSelectedGameObject(buttonOther.gameObject);
    }

    private void PlayerSpeaking(int choices)
    {
        button1.gameObject.SetActive(true);
        button2.gameObject.SetActive(true);
        if (!selfTalk)
        {
            buttonOther.interactable = false;
        }
        buttonLight.interactable = false;
        if (choices==3)
        {
            button3.gameObject.SetActive(false);
        }
        EventSystem.current.SetSelectedGameObject(button1.gameObject);
    }

    private void LightSpeaking()
    {
        buttonLight.gameObject.SetActive(true);
        buttonLight.interactable = true;
        if (!selfTalk)
        {
            buttonOther.interactable = false;
        }

        button1.gameObject.SetActive(false);
        button2.gameObject.SetActive(false);
        button3.gameObject.SetActive(false);
        EventSystem.current.SetSelectedGameObject(buttonLight.gameObject);
    }

    private void PlayerMadeChoice()
    {
        button1.gameObject.SetActive(false);
        button2.gameObject.SetActive(false);
        button3.gameObject.SetActive(false);
        buttonLight.gameObject.SetActive(false);
        if (!selfTalk)
        {
            buttonOther.gameObject.SetActive(false);
        }
    }

    private void FinishDialogue()
    {
        button1.gameObject.SetActive(false);
        button2.gameObject.SetActive(false);
        button3.gameObject.SetActive(false);
        buttonLight.gameObject.SetActive(false);
        if (!selfTalk)
        {
            buttonOther.gameObject.SetActive(false);
        }
        dialogueInProgress = false;
        GameMaster.EnablePlayerMovement(true);
    }

    public void Test1()
    {
        Debug.Log("WATER SPAWNS");
    }

    public void test2()
    {
        Debug.Log("+5 TO BAD");
    }


    public void setDialogueScript(DialogueScript script)
    {
        dialogueScript = script;
    }
}
