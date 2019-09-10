using UnityEngine;
using XNodeEditor;

namespace Dialogue.Nodes.Editor
{
    [CustomNodeEditor(typeof(DialogueNode))]
    public class DialogueNodeEditor : NodeEditor
    {
        public override void OnHeaderGUI()
        {
            GUI.color = Color.white;
            DialogueNode node = (DialogueNode)target;
            DialogueGraph graph = (DialogueGraph)node.graph;
            if (graph.GetCurrent() == node) GUI.color = Color.green;
            if (graph.GetEntryPoint() == node) GUI.color = Color.blue;
            string title = target.name;
            GUILayout.Label(title, NodeEditorResources.styles.nodeHeader, GUILayout.Height(30));
            GUI.color = Color.white;
        }

        public override void OnBodyGUI()
        {
            base.OnBodyGUI();
            DialogueNode node = (DialogueNode)target;
            DialogueGraph graph = (DialogueGraph)node.graph;
            //if (GUILayout.Button("Set as entry point")) graph.SetEntryPoint(node);
        }
    }
}
