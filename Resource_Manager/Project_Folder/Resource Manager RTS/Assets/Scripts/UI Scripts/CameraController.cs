using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        MoveCamera();
        ZoomCamera();
    }
    void MoveCamera()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(moveHorizontal, moveVertical);
        transform.Translate(moveHorizontal, moveVertical, 0.0f);
    }
    void ZoomCamera()
    {
        if (Input.mouseScrollDelta.y < 0)
        {
            gameObject.GetComponent<Camera>().orthographicSize++;
        }
        if (Input.mouseScrollDelta.y > 0)
        {
            gameObject.GetComponent<Camera>().orthographicSize--;
        }
    }
}
