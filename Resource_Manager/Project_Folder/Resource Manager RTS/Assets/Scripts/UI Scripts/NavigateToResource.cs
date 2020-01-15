using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NavigateToResource : MonoBehaviour
{
    public InputField infNavigate;
    public Camera mainCamera;
    // Start is called before the first frame update
    public void Navigate()
    {
         foreach(Node headNode in NodeInitializer.headNodes)
        {
            //Comb through each headNode to find the node we are looking for.
            FindNode(headNode);
        }
    }
    void FindNode(Node node)
    {
        if (node.nodeName == infNavigate.text)
        {
            //Move the camera to the node. Maintain Z axis value
            mainCamera.transform.position = new Vector3(node.location.x,node.location.y, -10.0f);
        }else
        {
            foreach (Node childNode in node.children)
            {
                //Search in all children
                FindNode(childNode);
            }
        }
    }
}
