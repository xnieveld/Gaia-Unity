using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
/*public class JSONReader : MonoBehaviour
{

    public static string ReadJSON(string fileName)
    {
        string path = "Assets\\" + fileName + ".json";
        StreamReader reader = new StreamReader(path);

        List<char[]> charList = new List<char[]>();
        int result = 1;
        int index = 0;
        while (result != 0)
        {

        }
        char[] charBuff = new char[2147483648];
        string json = reader.ReadBlock(charBuff, 0, 2147483647);
        reader.Close();
        return json;
    }

    public static StarList ParseStarJSON(string json)
    {
        StarList starList = JsonUtility.FromJson<StarList>(json);
        return starList;
    }
}
*/

public class JSONReader : MonoBehaviour {

    public static string ReadJSON(string fileName)
    {
        string path = "Assets\\" + fileName + ".json";
        StreamReader reader = new StreamReader(path);
        string json = reader.ReadToEnd();
        reader.Close();
        return json;
    }

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

    public static string[] ReadJSONFiles(string fileBaseName, int amount)
    {
        List<string> jsonParts = new List<string>();
        for (int i = 0; i < amount; i++)
        {
            jsonParts.Add(ReadJSON(fileBaseName + i.ToString()));
        }
        return jsonParts.ToArray();
    }
    
    public static StarList ParseStarJSON(string json)
    {
        StarList starList = JsonUtility.FromJson<StarList>(json);
        return starList;
    }
}
