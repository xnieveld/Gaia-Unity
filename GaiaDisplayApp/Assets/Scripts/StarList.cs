using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

/// <summary>
/// A class containing a list of stars
/// </summary>
[Serializable]
public class StarList {
    //public List<MetaData> metadata;
    public List<Star> data;

    public int Count
    {
        get
        {
            return data.Count;
        }
    }


    /// <summary>
    /// Get a list of basic stars to send to the GPU
    /// </summary>
    public List<Star> Stars
    {
        get
        {
            return data;
        }
    }

    /// <summary>
    /// Merge another startlist into this starlist.
    /// </summary>
    /// <param name="newList">The starlist to merge into this one.</param>
    public void Merge(StarList newList)
    {
        if (data == null)
        {
            data = new List<Star>();
        }
        data.AddRange(newList.data);
    }
}
