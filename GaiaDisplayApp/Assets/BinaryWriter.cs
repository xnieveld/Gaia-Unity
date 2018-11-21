using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class BinaryWriter {
    public static void WriteGalacticObject(GalacticObject go)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = new FileStream(Application.dataPath + "/octree.oct", FileMode.Create);

        GalacticObjectData data = new GalacticObjectData(go);

        bf.Serialize(stream, data);

        stream.Close();
    }

    public static GalacticObject ReadGalacticObject()
    {
        GalacticObject new_go = new GalacticObject();

        if (File.Exists(Application.dataPath + "/octree.oct")) {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(Application.dataPath + "/octree.oct", FileMode.Open);

            GalacticObjectData data = bf.Deserialize(stream) as GalacticObjectData;
            stream.Close();

            new_go.coordinates.x = data.coordinates[0];
            new_go.coordinates.y = data.coordinates[1];
            new_go.coordinates.z = data.coordinates[2];

            new_go.colour.x = data.colour[0];
            new_go.colour.y = data.colour[1];
            new_go.colour.z = data.colour[2];
            new_go.colour.w = data.colour[3];

            new_go.magnitude = data.magnitude;
        } else
        {
            Debug.LogError("Load File does not excist");
        }

        return new_go;
    }
}

[Serializable]
public class GalacticObjectData
{
    public readonly float[] coordinates = new float[3];
    public readonly float[] colour = new float[4];
    public readonly float magnitude;

    public GalacticObjectData(GalacticObject go)
    {
        coordinates[0] = go.coordinates.x;
        coordinates[1] = go.coordinates.y;
        coordinates[2] = go.coordinates.z;

        colour[0] = go.colour.x;
        colour[1] = go.colour.y;
        colour[2] = go.colour.z;
        colour[3] = go.colour.w;

        magnitude = go.magnitude;
    }
}