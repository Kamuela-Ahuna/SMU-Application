using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public ArrayList children = new ArrayList();
    public bool active = true;
    public string nodeName;
    public Vector2 location;
    public string description; //Only implement if the assignment gets finished. 
    public static ArrayList totalNodes = new ArrayList();

    public Node(string newName)
    {
        nodeName = newName;
        totalNodes.Add(newName);
    }

    public void AddNodeToChildren(string childName, string parentName)
    {
        /*For this method, we need to check if this Node is the parent. If it is, 
         * then we can simply assign it to the children nodes. If it is not, we need to
         * see if the parent node is any of the children. If it is, call this method in that child. 
         * If it isn't, call this method in all children. (Maybe make this method a booelan that returns 
         * true if we ended up assigning the child. That could be our termination condition.)
         */
        Debug.Log("running on " + this.nodeName + " With the child as " + childName);
        //parentName is the name of the object we are looking for to assign it a child. 

        //if the parent we're looking for is this object, add it to the children ArrayList
         if (parentName == this.nodeName)
        {
            children.Add(new Node(childName));
            Debug.Log("Successfully Assigned " + childName + " to " + this.nodeName);
        }
         //Otherwise, check children for the name. 
        else
        {
            //Check if the node was added to any of the children of this object. 
            bool nodeAdded = false;
            //Comb through the arraylist of children.
            foreach(Node childNode in children)
            {
                //If any of the children have the name we're looking for. 
                if (childNode.nodeName == parentName)
                {
                    //Add them to the ArrayList and flip our bool
                    childNode.children.Add(new Node(childName));
                    nodeAdded = true;
                    Debug.Log("Successfully Assigned " + childName + " to " + childNode.nodeName);
                    break;
                }
            }
            //if none of the children have the name we're looking for
            if (!nodeAdded)
            {
                //run this method again in every child. 
                foreach (Node childNode in children)
                {
                    childNode.AddNodeToChildren(childName, parentName);
                }
            }
        }
         /* The current problem with this method is that we don't recursively reduce the load
          * with each method call. This could lead to some insane performance times. Look at 
          * refactoring this before the week is over. 
          */
    }
    //Every node we have 
    public void Deactivate()
    {
        active = false;
        foreach(Node child in children)
        {
            child.Deactivate();
        }
    }
    public void Activate()
    {
        active = true;
        foreach (Node child in children)
        {
            child.Activate();
        }
    }
    public void Print()
    {
        Debug.Log(nodeName + " is the parent for " + children.Count);
        foreach (Node node in children)
            node.Print();
    }
}
