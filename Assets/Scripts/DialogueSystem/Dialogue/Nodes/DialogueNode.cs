using System;
using UnityEngine;
using XNode;
namespace Dialogue.Nodes
{
    public abstract class DialogueNode : Node
    {
        public abstract void MoveNext();

        public void OnEnter()
        {
            DialogueGraph fmGraph = (DialogueGraph)graph;
            fmGraph.SetCurrent(this); // TODO this feels scary but it might work, just a reminder that this is a place to look when getting bugs.
        }

        [Serializable]
        public class Empty { }
    }
}
