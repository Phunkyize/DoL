using UnityEngine;
using XNode;
namespace Dialogue.Nodes
{
    public class StartNode : DialogueNode
    {
        [SerializeField, Output] private Empty output;
        // Start is called before the first frame update
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
       
        public override object GetValue(NodePort port)
        {
            return null;
        }
    }
}
