using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ReadInResources : MonoBehaviour
{
    //This will read the resource file in from the desired path
    public string path = "";
    private StreamReader sr;

    //This will store the resource.txt entries as a table of keys and values
    public Hashtable resources = new Hashtable();

    //This will store any resources that don't rely on other resources, i.e. headNodes.
    public ArrayList headNodeNames = new ArrayList();
    public ArrayList allNodeNames = new ArrayList();
    public PathReader pathReader;

    public bool CheckPath()
    {
        try
        {
            try
            {
                sr = new StreamReader(path);
                sr.Peek();
            }
            catch (DirectoryNotFoundException e)
            {
                pathReader.pathText = e.Message;
                return false;
            }
        }
        catch (FileNotFoundException)
        {
            Debug.Log("Path not found");
            return false;
        }
        return true;
    }
    // Start is called before the first frame update
    public void ReadAndStore()
    {
        sr = new StreamReader(path);
        //First store the text file as useable data
        ParseFile();
        //Then look for any headNodes. 
        CheckForHeadNodes();
        StoreUniqueValues();
    }
    void ParseFile()
    {
        //Read the first line of code
        string data = sr.ReadLine();

        //While we still have data to read, continue to parse the data
        while (data != null)
        {
            Debug.Log(data);
            //Split values using the space as our indexer
            string[] values = data.Split(' ');

            //Store the split values in the Hashtable as keys and values respectively.
            resources.Add(values[0], values[1]);

            //Read the next line.
            data = sr.ReadLine();
        }
        foreach (DictionaryEntry item in resources)
            Debug.Log("Key: " + item.Key + " Value: " + item.Value);
    }
    void CheckForHeadNodes()
    {
        /*
         * The way the text document is set up, if we have values that exist on the right
         * but don't exist on the left, then those values are head nodes. So, in this method,
         * we will store each item in the values section of our hashtable as a unique item in 
         * a temporary Arraylist (ArrayList so it can be done with any number of resources). 
         * We will then use this temporary ArrayList and check through the keys of the same 
         * Hashtable. If the item does not exist in the keys of the Hashtable, it must be a Headnode. 
         * These values will be stored in the Headnodes ArrayList for later use.
         */

        //Temporary ArrayList (See above)
        ArrayList tempItems = new ArrayList();

        //Comb through each resourcePair in the resources Hashtable
        foreach(DictionaryEntry resourcePair in resources)
        {
            //Use this to flag any duplicates
            bool valuePresent = false;

            //Comb through the temporary ArrayList
            for (int i = 0; i < tempItems.Count; i++)
            {
                //Check for duplicates. We want to store the values uniquely. 
                if (string.Compare(resourcePair.Value.ToString(),tempItems[i].ToString()) == 0)
                    valuePresent = true;
            }

            //If the value isn't already present in the temporary ArrayList, store it. 
            if (!valuePresent)
                tempItems.Add(resourcePair.Value);
        }
        //Now we will comb through both containers again and see if the values are present in the Keys of the Hashtable.
        foreach (var item in tempItems)
        {
            //Using a bool to check again
            bool valuePresent = false;

            //Comb through the keys
            foreach (DictionaryEntry resource in resources)
            {
                if (string.Compare(item.ToString(), resource.Key.ToString()) == 0){
                    valuePresent = true;
                }
            }
            //If the value doesn't exist in the keys, it is a headnode. Store it. 
            if (!valuePresent)
                headNodeNames.Add(item);
        }

        foreach (var item in headNodeNames)
            Debug.Log("Head nodes: " + item);
    }
    void StoreUniqueValues()
    {
        //Store all of the values uniquely for later use.
        foreach (DictionaryEntry item in resources)
        {
            bool keyPresent = false;
            bool valuePresent = false;
            for (int i = 0; i < allNodeNames.Count; i++)
            {
                if (string.Compare(item.Key.ToString(), allNodeNames[i].ToString()) == 0){
                    keyPresent = true;
                }
                if (string.Compare(item.Value.ToString(), allNodeNames[i].ToString()) == 0)
                {
                    valuePresent = true;
                }
            }
            if (!keyPresent)
                allNodeNames.Add(item.Key);
            if (!valuePresent)
                allNodeNames.Add(item.Value);
        }
    }
}
