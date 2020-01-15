using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PathReader : MonoBehaviour
{
    public InputField infResourcePath;
    public ReadInResources resourceReader;
    public GameObject panel;
    public TMP_Text tmp;
    public string pathText;

    public void FindPath()
    {
        resourceReader.path = infResourcePath.text;
        if (resourceReader.CheckPath())
        {
            Debug.Log("Path is clear");
            panel.SetActive(false);
            resourceReader.ReadAndStore();
            TextFadeout.fadeOut = true;
            UpdateText(Color.green, "Good to go! Initialize the Nodes!");
        }
        else
        {
            TextFadeout.fadeOut = false;
            UpdateText(Color.red, pathText);
        }
    }
    void UpdateText(Color color, string value)
    {
        tmp.gameObject.SetActive(false);
        tmp.gameObject.SetActive(true);
        tmp.color = color;
        tmp.text = value;
    }
}
