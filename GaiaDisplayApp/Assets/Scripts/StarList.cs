using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarList {
    public List<MetaData> metadata;
    public List<Star> data;

    public Vector4[] StarCoords
    {
        get
        {
            Vector4[] coords = new Vector4[25600];
            for (int i = 0; i < data.Count; i++)
            {
                coords[i] = new Vector4((float)data[i].RightAccension, (float)data[i].Inclination+90, (float)data[i].Distance);
                //Debug.Log(new Vector3(Mathf.Round(coords[i].x), Mathf.Round(coords[i].y), Mathf.Round(coords[i].z)));
            }
            return coords;
        }
    }

    public float[] StarMags
    {
        get
        {
            float[] mags = new float[25600];
            for(int i = 0; i < data.Count; i++)
            {
                mags[i] = data[i].Magnitude;
            }
            return mags;
        }
    }

    public Vector4[] StarColours
    {
        get
        {
            Vector4[] colours = new Vector4[25600];
            for (int i = 0; i < data.Count; i++)
            {
                colours[i] = new Vector4(1,1,1,1);
            }
            colours[0] = new Vector4(20, 10, 10, 1);
            colours[100] = new Vector4(200, 100, 1000, 1);
            return colours;
        }
    }
}
