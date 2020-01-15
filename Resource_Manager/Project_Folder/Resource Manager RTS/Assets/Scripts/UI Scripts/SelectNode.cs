using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectNode : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        RaycastToMousePoint();
    }
    void RaycastToMousePoint()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition),Vector2.zero);
        if (hit.collider != null)
        {
            hit.collider.gameObject.GetComponentInParent<NodeUIHandler>().ToggleNodeActive();
        }
    }
}
