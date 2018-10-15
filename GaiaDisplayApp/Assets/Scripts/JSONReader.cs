using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class JSONReader : MonoBehaviour {

    public static string ReadJSON(string fileName)
    {
        string path = "Assets\\" + fileName + ".json";
        StreamReader reader = new StreamReader(path);
        string json = reader.ReadToEnd();
        reader.Close();
        return json;
    }
    
    public static StarList ParseStarJSON(string json)
    {
        StarList starList = JsonUtility.FromJson<StarList>(json);
        return starList;
    }
}
