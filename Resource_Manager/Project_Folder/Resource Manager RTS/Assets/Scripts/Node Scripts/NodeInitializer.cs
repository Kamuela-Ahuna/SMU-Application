using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NodeInitializer : MonoBehaviour
{
    [SerializeField]
    private ReadInResources resourceReader;
    public static ArrayList headNodes = new ArrayList();
    public static bool nodesInitialized = false;
    private TMP_Text tmpText;

    private void Start()
    {
        tmpText = GameObject.FindWithTag("DisplayText").GetComponent<TMP_Text>();
    }
    void UpdateText(Color color, string value)
    {
        tmpText.gameObject.SetActive(true);
        tmpText.color = color;
        tmpText.text = value;
    }
    public void RunNodeInitialization()
    {
        InitializeHeadNodes();
        AssignChildrenToHeadNodes();
        InitializeRemainingNodes();
        FindStragglers();
        PrintNodes();
        nodesInitialized = true;
        UpdateText(Color.green, "Nodes have been Initialized!");
    }

    public void InitializeHeadNodes()
    {
        //Cycle through the node names and create nodes for them. 
        foreach (var item in resourceReader.headNodeNames)
            headNodes.Add(new Node(item.ToString()));
    }
    public void AssignChildrenToHeadNodes()
    {
        foreach (Node node in headNodes)
        {
            foreach (DictionaryEntry nodePair in resourceReader.resources)
            {
                if (string.Compare(node.nodeName,nodePair.Value.ToString()) == 0)
                {
                    Debug.Log("Attempting to Assign " + nodePair.Key.ToString() + " to " + nodePair.Value.ToString());
                    //See what resources rely on the head nodes and assign them as children. 
                    node.AddNodeToChildren(nodePair.Key.ToString(), nodePair.Value.ToString());
                }
            }
        }
    }
    public void InitializeRemainingNodes()
    {
        foreach (Node node in headNodes)
        {
            foreach (DictionaryEntry nodePair in resourceReader.resources)
            {
                if (string.Compare(node.nodeName, nodePair.Value.ToString()) != 0)
                {
                    //Any resources that don't rely directly on our head nodes rely on children of the head nodes. 
                    //Assign them here. 
                    Debug.Log("Attempting to assign " + nodePair.Key.ToString() + " to " + nodePair.Value.ToString());
                    node.AddNodeToChildren(nodePair.Key.ToString(), nodePair.Value.ToString());
                }
            }
        }
    }
    public void PrintNodes()
    {
        foreach (Node node in headNodes)
            node.Print();
    }
    public void FindStragglers()
    {
        Debug.Log("Checking For Stragglers");
        Debug.Log(resourceReader.allNodeNames.Count + "," + Node.totalNodes.Count);
        while (resourceReader.allNodeNames.Count > Node.totalNodes.Count)
        {
            Debug.Log("Stragglers detected");
            foreach (var item in resourceReader.allNodeNames)
            {
                bool itemPresent = false;
                foreach (var node in Node.totalNodes)
                {
                    if (string.Compare(item.ToString(), node.ToString()) == 0)
                        itemPresent = true;
                }
                if (!itemPresent)
                {
                    foreach(Node node in headNodes)
                    {
                        string parentName = (string)resourceReader.resources[item.ToString()];
                        node.AddNodeToChildren(item.ToString(), parentName);
                    }
                }
            }
        }
    }
    /*
     *This program needs to read through the headNodes arrayList in ReadInReasources (Hereafter known as RIR)
     * When we read through the headnodes, we need to check them to see what their children are. We should assign 
     * the children to the headnodes then repeat the process with every one of its children. 
     */
}
