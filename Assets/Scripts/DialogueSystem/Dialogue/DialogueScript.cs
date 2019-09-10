using System;
using Dialogue;
using UnityEngine;

[Serializable]
public class DialogueScript
{
    [SerializeField] private DialogueGraph dialogue = null;

    public DialogueScript()
    {

    }

    public DialogueGraph GetDialogueGraph()
    {
        return dialogue;
    }
}
