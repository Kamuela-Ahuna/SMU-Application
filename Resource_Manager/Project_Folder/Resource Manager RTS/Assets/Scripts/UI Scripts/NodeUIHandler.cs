using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NodeUIHandler : MonoBehaviour
{
    public Node myNode;
    private bool wasActive;

    public GameObject activeUI;
    public GameObject inactiveUI;
    private void Start()
    {
        GetComponent<TextMeshPro>().text = myNode.nodeName;
    }
    private void Update()
    {
        if (wasActive && !myNode.active)
        {
            inactiveUI.SetActive(true);
            activeUI.SetActive(false);
        }else if(!wasActive && myNode.active)
        {
            inactiveUI.SetActive(false);
            activeUI.SetActive(true);
        }

        wasActive = myNode.active;
    }
    public void ToggleNodeActive()
    {
        if (myNode.active)
            myNode.Deactivate();
        else if (!myNode.active)
            myNode.Activate();
    }
}
