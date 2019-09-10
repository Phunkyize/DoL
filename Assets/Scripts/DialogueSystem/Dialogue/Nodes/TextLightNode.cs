using UnityEngine;
using XNode;

namespace Dialogue.Nodes
{
    public class TextLightNode : DialogueNode
    {
        [SerializeField, Input] private Empty input;
        [SerializeField, Output] private Empty output;
        [SerializeField, TextArea] private string text = "";

        public override void MoveNext()
        {
            DialogueGraph fmGraph = (DialogueGraph)graph;

            if (fmGraph.GetCurrent() != this)
            {
                Debug.LogWarning("Node isn't active");
                return;
            }

            NodePort exitPort = GetOutputPort("output");

            if (!exitPort.IsConnected)
            {
                Debug.LogWarning("Node isn't connected");
                return;
            }

            DialogueNode node = (DialogueNode)exitPort.Connection.node;
            node.OnEnter();
        }
        public string GetText()
        {
            return text;
        }
        public override object GetValue(NodePort port)
        {
            return null;
        }
    }
}

