using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class BinaryManager : MonoBehaviour {


    const string folderName = "BinaryStarData";
    const string fileExtension = ".stl";

    public static void StarListToBinary(StarList starList, string fileName)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();

        using (FileStream fileStream = File.Open(fileName + fileExtension, FileMode.OpenOrCreate))
        {
            binaryFormatter.Serialize(fileStream, starList);
        }
    }

    public static StarList BinaryToStarList(string fileName)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();

        using (FileStream fileStream = File.Open(fileName + fileExtension, FileMode.Open))
        {
            return (StarList)binaryFormatter.Deserialize(fileStream);
        }
    }

    public static void StarListToBinaryOwn(StarList starList, string fileName)
    {
        byte[][] ba = new byte[starList.Count][];
        for (int i = 0; i <  starList.Count; i++)
        {
            ba[i] = (byte[])starList.data[i];   
        }
    }
}
