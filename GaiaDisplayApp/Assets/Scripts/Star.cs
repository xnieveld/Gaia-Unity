using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// A star class. MUST be identical to Star struct in StarListShader.compute
/// </summary>
[Serializable]
public struct Star
{  
    
    public float x; 
    public float y;
    public float z;
    public float r; //red
    public float g; //green
    public float b; //blue
    public float m; //aboslute magnitude

    /// <summary>
    /// Size of a basic star
    /// </summary>
    public static int ByteSize
    {
        get
        {
            return 120;
        }
    }

   

    /// <summary>
    /// Cast a star into a byte array.
    /// </summary>
    /// <param name="s"></param>
    public static explicit operator Byte[](Star s)
    {
        Byte[] ba = new byte[ByteSize];        
        ba = (byte[])s;
        return ba;
    }


    /// <summary>
    /// Make a new star
    /// </summary>
    /// <param name="x">X position, Earth = 0 </param>
    /// <param name="y">Y position, Earth = 0 </param>
    /// <param name="z">Z position, Earth = 0 </param>
    /// <param name="m">Absolute Magnitude of the star</param>
    /// <param name="r">Red color component</param>
    /// <param name="g">Green color component</param>
    /// <param name="b">Blue color component</param>
    public Star(float x, float y, float z, float m, float r, float g, float b)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        this.r = r;
        this.g = g;
        this.b = b;
        this.m = m;
    }
}
