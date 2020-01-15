using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayNodes : MonoBehaviour
{
    public GameObject nodePrefab;

    public float distancer;
    public float safeZone;

    public GameObject linkManager;
    public static bool nodesDisplayed = false;
    private TMP_Text displayText;

    private ArrayList activeUIElements = new ArrayList();
    private void Start()
    {
        displayText = GameObject.FindWithTag("DisplayText").GetComponent<TMP_Text>();
    }
    public void DisplayAllNodes()
    {
        if (NodeInitializer.nodesInitialized)
        {
            //Delete any UI elements that exist.
            DeleteAllActiveUIElements();
            activeUIElements.Clear();

            //The distance between each resource tree
            float distanceMultiplier = 0.0f;
            foreach (Node node in NodeInitializer.headNodes)
            {
                //Find a point to display a node
                FindPoint(node, new Vector2(10.0f, 10.0f) * distanceMultiplier);
                //Give some space for the next resource tree
                distanceMultiplier += 2.0f;
            }
            foreach (Node node in NodeInitializer.headNodes)
            {
                //Once the nodes exist, bring in the links
                DrawLineToChild(node);
            }
            nodesDisplayed = true;
        }
        else
        {
            UpdateText(Color.red, "You must initialize the nodes first!");
        }
    }
    void UpdateText(Color color, string value)
    {
        displayText.gameObject.SetActive(true);
        displayText.color = color;
        displayText.text = value;
    }
    void FindPoint(Node node, Vector2 origin)
    {
        //We don't want to get caught in the loop so we'll create a failsafe for that. 
        int numberOfChecks = 0;
        Vector2 point = origin;
        //We may have to resize the safeZone and distancer, so store their initial values for later. 
        float defaultDistance = distancer;
        float defaultSafeZone = safeZone;

        do
        {
            //If we do this too much, we'll adjust our values
            //This should keep us from getting caught in the loop
            if (numberOfChecks > 100)
            {
                distancer++;
                safeZone--;
            }
            numberOfChecks++;
            Debug.Log(numberOfChecks);
            point = origin + Random.insideUnitCircle * distancer;
        } while (!PointIsSafe(node,point));

        numberOfChecks = 0;
        safeZone = defaultSafeZone;
        distancer = defaultDistance;

        CreateNodeObject(node, point);

        foreach(Node childNode in node.children)
        {
            FindPoint(childNode, point);
        }
    }
    bool PointIsSafe(Node node,Vector2 point)
    {
        Debug.Log("Checking if safe...");
        //See if there are any colliders in our way
        Collider2D[] colliders = Physics2D.OverlapCircleAll(point, safeZone);
        if ( colliders.Length != 0)
        {
            Debug.Log(node.nodeName + " Has encountered a collision");
            return false;
        }
        else
        {
            Debug.Log(node.nodeName + " is safe to spawn");
            return true;
        }
    }
    void CreateNodeObject(Node node, Vector2 location)
    {
        GameObject clone;
        clone = Instantiate(nodePrefab, location, nodePrefab.transform.rotation);
        NodeUIHandler nodeUI = clone.GetComponent<NodeUIHandler>();
        nodeUI.myNode = node;
        nodeUI.myNode.location = location;
        activeUIElements.Add(clone);
    }
    void DeleteAllActiveUIElements()
    {
        foreach (GameObject nodeObject in activeUIElements)
            Destroy(nodeObject);
    }

    void DrawLineToChild(Node node)
    {
        foreach(Node childNode in node.children)
        {
            GameObject LMclone;
            LMclone = Instantiate(linkManager, Vector2.zero, linkManager.transform.rotation);
            LineRenderer link = LMclone.GetComponent<LineRenderer>();
            link.startWidth = .5f;
            link.endWidth = .01f;
            link.positionCount = 2;
            link.SetPosition(0, node.location);
            link.SetPosition(1, childNode.location);
            activeUIElements.Add(LMclone);
            DrawLineToChild(childNode);
        }
    }
    /*this script needs to go through our node tree and find a space for each individual node.
    To do this, we first need to find a safe spot for all of our head nodes. This should be a 
    significant distance from other head nodes, but the first one should start at origin.
     */
}
