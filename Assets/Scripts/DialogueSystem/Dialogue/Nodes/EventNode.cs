using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using XNode;

namespace Dialogue.Nodes
{
    public class EventNode : DialogueNode
    {
        [SerializeField, Input] private Empty input;
        [SerializeField, Output] private Empty output;
        [SerializeField] public UnityEvent[] trigger;


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

        public void Trigger()
        {
            for (int i = 0; i < trigger.Length; i++)
            {
                trigger[i].Invoke();
            }
        }

        public override object GetValue(NodePort port)
        {
            return null;
        }
    }
}
