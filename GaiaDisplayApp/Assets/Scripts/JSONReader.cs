using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// A JSON reader for reading in the stars. 
/// </summary>
public class JSONReader : MonoBehaviour {

    /// <summary>
    /// Read a single JSON file (Or any file for that matter). 
    /// </summary>
    /// <param name="fileName">The name of the file</param>
    /// <returns>Returns the content of the file as one long string</returns>
    public static string ReadJSON(string fileName)
    {
        string path = "Assets\\" + fileName + ".json";
        StreamReader reader = new StreamReader(path);
        string json = reader.ReadToEnd();
        reader.Close();
        return json;
    }

    /// <summary>
    /// Read a very long JSON file (or any file).
    /// </summary>
    /// <param name="fileName">The name of the file</param>
    /// <returns>Returns an array of max-length strings containing the file in pieces.</returns>
    public static string[] ReadLongJSON(string fileName)
    {
        string path = "Assets\\" + fileName + ".json";
        StreamReader reader = new StreamReader(path);
        List<string> jsonParts = new List<string>();
        int stringIndex = 0;
        char[] currentCharBuffer;
        int lastLength = int.MaxValue;

        while (lastLength == int.MaxValue)
        {
            currentCharBuffer = new char[int.MaxValue];
            lastLength = reader.ReadBlock(currentCharBuffer, stringIndex * int.MaxValue, int.MaxValue);

            jsonParts.Add(currentCharBuffer.ToString());
            stringIndex++;
        }

        reader.Close();
        return jsonParts.ToArray();
    }

    /// <summary>
    /// Read a list of JSON strings into a StarList
    /// </summary>
    /// <param name="jsons">A list of JSON stars.</param>
    /// <returns>A StarList object containing all the stars read.</returns>
    public static StarList ParseStarJSONList(string[] jsons)
    {
        StarList returnList = new StarList();
        foreach (string json in jsons)
        {
            returnList.Merge(JsonUtility.FromJson<StarList>(json));
        }
        Debug.Log(returnList.Count);
        return returnList;
    }

    /// <summary>
    /// Read a list of JSON files (or any text files)
    /// </summary>
    /// <param name="fileBaseName">Base name of the files, files are expected to have numerical indicators after this name. For example 'file0', 'file1', 'file2'.</param>
    /// <param name="amount">Number of files to read.</param>
    /// <returns>An array of strings containing all the files. </returns>
    public static string[] ReadJSONFiles(string fileBaseName, int amount)
    {
        List<string> jsonParts = new List<string>();
        for (int i = 0; i < amount; i++)
        {
            jsonParts.Add(ReadJSON(fileBaseName + i.ToString()));
        }
        return jsonParts.ToArray();
    }
    
    /// <summary>
    /// Parse a JSON string into a Starlist.
    /// </summary>
    /// <param name="json">JSON string to be parsed.</param>
    /// <returns>Starlist object.</returns>
    public static StarList ParseStarJSON(string json)
    {
        StarList starList = JsonUtility.FromJson<StarList>(json);
        return starList;
    }
}
