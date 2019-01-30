using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

/// <summary>
/// Attempt at a binary reader/writer, but turned out to be really, really slow. Abandoned.
/// </summary>
public class BinaryManager : MonoBehaviour {

    /// <summary>
    /// Folder name of where the files are stored
    /// </summary>
    const string folderName = "BinaryStarData";

    /// <summary>
    /// File extention, stl = STarList
    /// </summary>
    const string fileExtension = ".stl";


    /// <summary>
    /// Convert a starlist object to a binary file
    /// </summary>
    /// <param name="starList">The starList to convert</param>
    /// <param name="fileName">The filename to save it to.</param>
    public static void StarListToBinary(StarList starList, string fileName)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();

        using (FileStream fileStream = File.Open(fileName + fileExtension, FileMode.OpenOrCreate))
        {
            binaryFormatter.Serialize(fileStream, starList);
        }
    }

    /// <summary>
    /// Convert a binary file to a starList object.
    /// </summary>
    /// <param name="fileName">The filename</param>
    /// <returns>The read in starList.</returns>
    public static StarList BinaryToStarList(string fileName)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();

        using (FileStream fileStream = File.Open(fileName + fileExtension, FileMode.Open))
        {
            return (StarList)binaryFormatter.Deserialize(fileStream);
        }
    }


    /// <summary>
    /// WIP, convert the a starlist to own binary format, but not finished, and not needed anymore.
    /// </summary>
    /// <param name="starList">The starlist to convert</param>
    /// <param name="fileName">The filename to save it to.</param>
    public static void StarListToBinaryOwn(StarList starList, string fileName)
    {
        byte[][] ba = new byte[starList.Count][];
        for (int i = 0; i <  starList.Count; i++)
        {
            ba[i] = (byte[])starList.data[i];   
        }
    }
}
