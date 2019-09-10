using UnityEngine;
using XNode;

namespace Dialogue.Nodes
{
    public class ChoiceNode : DialogueNode
    {
        [SerializeField, Input] private Empty input;
        [Output] public Empty out1;
        [Output] public Empty out2;
        [Output] public Empty out3;

        [SerializeField] private string choice1 = "";
        [SerializeField] private string choice2 = "";
        [SerializeField] private string choice3 = "";

        private int choice = -1;

        public override void MoveNext()
        {
            // Do not continue if the player hasn't made a choice
            if (choice != 0 && choice != 1 && choice != 2)
            {
                Debug.LogWarning("Cannot continue, no choice has been made");
                return;
            }

            DialogueGraph fmGraph = (DialogueGraph)graph;

            if (fmGraph.GetCurrent() != this)
            {
                Debug.LogWarning("Node isn't active");
                return;
            }
            NodePort exitPort;
            if (choice == 3)
            {
                exitPort = this.GetOutputPort("out3");
            }
            else
            {
                exitPort = this.GetOutputPort(choice == 0 ? "out1" : "out2");
            }

            if (!exitPort.IsConnected)
            {
                Debug.LogWarning("Node isn't connected");
                return;
            }

            DialogueNode node = (DialogueNode)exitPort.Connection.node;
            node.OnEnter();
        }

        public string GetChoiceText(int choice)
        {
            if (choice == 0)
            {
                return choice1;
            }
            if (choice == 1)
            {
                return choice2;
            }
            if(choice == 2)
            {
                return choice3;
            }

            Debug.LogWarning("Invalid binary choice, should be 1 or 0");
            return null;
        }

        public void SetChoice(int choice)
        {
            if (choice == 0 || choice == 1 || choice == 3)
            {
                this.choice = choice;
            }
            else
            {
                Debug.LogError("Binary choice, pass value of either 0 or 1. Was: " + choice);
            }
        }

        public override object GetValue(NodePort port)
        {
            return null;
        }
    }
}


