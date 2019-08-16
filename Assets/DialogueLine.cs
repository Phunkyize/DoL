using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    public Speaker speaker;
    public bool choice;
    public int goodChoice;
    public int badChoice;
    [TextArea(3, 10)]
    public string line;

}

public enum Speaker
{
    Player,
    Light,
    Other
}

