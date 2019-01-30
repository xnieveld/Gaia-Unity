using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Basically a Vector 3 but with doubles. 
/// </summary>

[Serializable]
public class DoubleVector3{
    [SerializeField]
    double x, y, z;

    /// <summary>
    /// Make a new 0 Double  Vector
    /// </summary>
    public DoubleVector3()
    {
        x = y = z = 0;
    }

    /// <summary>
    /// Make a new Double Vector with xyz values
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    public DoubleVector3(double x, double y, double z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    /// <summary>
    /// Convert Vector3 in DoubleVector3
    /// </summary>
    /// <param name="vec"></param>
    public DoubleVector3(Vector3 vec)
    {
        this.x = vec.x;
        this.y = vec.y;
        this.z = vec.z;
    }
    public DoubleVector3(Vector2 vec)
    {
        this.x = vec.x;
        this.y = vec.y;
        this.z = 0;
    }

    /// <summary>
    /// Cast a Vector3 (floats) to a DoubleVector3
    /// </summary>
    /// <param name="vec"></param>
    public static implicit operator DoubleVector3 (Vector3 vec)
    {
        DoubleVector3 dVec= new DoubleVector3();
        dVec.x = vec.x;
        dVec.y = vec.y;
        dVec.z = vec.z;
        return dVec;
    }
    
    /// <summary>
    /// Cast a DoubleVector3 to a Vector3 (floats, so lossy cast)
    /// </summary>
    /// <param name="vec"></param>
    public static explicit operator Vector3(DoubleVector3 vec)
    {
        Vector3 dVec = new Vector3();
        dVec.x = (float)vec.x;
        dVec.y = (float)vec.y;
        dVec.z = (float)vec.z;
        return dVec;
    }
   
    /// <summary>
    /// Calculate the length of the vector.
    /// </summary>
    /// <returns>the length</returns>
    public double Length()
    {
        return Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2) + Math.Pow(z, 2));
    }

    /// <summary>
    /// Calculates the length of the vector, but doesn't finish the calculation. Useful to prevent extra calculations in case the length cubed is needed.
    /// </summary>
    /// <returns>Length cubed</returns>
    public double LengthCubed()
    {
        return Math.Pow(x, 2) + Math.Pow(y, 2) + Math.Pow(z, 2);
    }
    public override string ToString()
    {
        return "(" + x + ", " + y + ", " + z + ")";
    }

    /// <summary>
    /// Add two doublevectors.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns>The sum of the two double vectors.</returns>
    public static DoubleVector3 operator +(DoubleVector3 a, DoubleVector3 b)
    {
        return new DoubleVector3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
    }

    /// <summary>
    /// Subtract two doublevectors.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns>Vector a - vector b.</returns>
    public static DoubleVector3 operator -(DoubleVector3 a, DoubleVector3 b)
    {
        return new DoubleVector3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
    }

    /// <summary>
    /// Multiply a double vector with a scalar. 
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns>The new double factor</returns>
    public static DoubleVector3 operator *(DoubleVector3 a, double b)
    {
        return new DoubleVector3(a.X * b, a.Y * b, a.Z * b);
    }


    /// <summary>
    /// Multiply a double vector with a scalar. 
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns>The new double factor</returns>
    public static DoubleVector3 operator *(double a, DoubleVector3 b)
    {
        return new DoubleVector3(a * b.X, a * b.Y, a * b.Z);
    }

    /// <summary>
    /// Divide a double vector by a scalar
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns>The double Vector divided by the scalar.</returns>
    public static DoubleVector3 operator /(DoubleVector3 a, double b)
    {
        return new DoubleVector3(a.X / b, a.Y / b, a.Z / b);
    }

    /// <summary>
    /// Create a new double vector with each component being scalar A divided by the components of Vector b
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns>A new Vector (a / b.x, a / b.y, a / b.z)</returns>
    public static DoubleVector3 operator /(double a, DoubleVector3 b)
    {
        return new DoubleVector3(a / b.X, a / b.Y, a / b.Z);
    }
    public double X
    {
        get { return x; }
        set { x = value; }
    }

    public double Y
    {
        get { return y; }
        set { y = value; }
    }

    public double Z
    {
        get { return z; }
        set { z = value; }
    }
}
