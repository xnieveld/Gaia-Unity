using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// A star class.
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
    /*
        /// <summary>
        /// Estimate temperature of star based on absolute magnitude, and the HR Diagram.
        /// </summary>
        /// <param name="absoluteMag"Absolute Magnitude></param>
        /// <returns>Educated guess to the temperature in kelvin</returns>
        public static float EstimateTemperature(float absoluteMag)
        {
            float temp = 0;
            if (absoluteMag > 12.5) {temp = Random.Range(1200,1800);}
            else if (absoluteMag > 10) {temp = Random.Range(2000, 3200);}
            else if (absoluteMag > 6) {temp = Random.Range(3000, 5000);}
            else if (absoluteMag > 4) {
                temp = Random.Range(4500, 6500);
                if (Random.nextFloat < 0.1) {temp = 2500;}
            }
            else if (absoluteMag > 2) {
                temp = Random.Range(4800, 7500);
                if (Random.nextFloat < 0.1) {temp = Random.Range(2000, 6000);}
            }
            else if (absoluteMag > -1) {
                temp = Random.Range(7000, 12000);
                if (Random.nextFloat < 0.5) {temp = Random.Range(1000, 6500);}
            }
            else {
                temp = Random.Range(10000, 40000);
                if (Random.nextFloat < 0.3) {temp = Random.Range(5000, 12000);}
            }
            return temp;
        }*/

    /// <summary>
    /// Calculate an RBG colour from a given temperature
    /// </summary>
    /// <param name="t">Temperature in Kelvin between 1000 and 40000</param>
    /// <returns>RGB Aproximate equivalent</returns>
    public static Color ColorTemperature(float t)
    {
        Color color = new Color(0,0,0);

        float temperatureClamped = Mathf.Clamp(t, 1000, 40000);
        temperatureClamped /= 100;
        if (temperatureClamped <= 66)
        {
            color.r += 255;
            color.g += Mathf.Clamp(99.4709025861f * Mathf.Log(temperatureClamped) - 161.1195681661f, 0, 255);
            if (temperatureClamped > 19)
            {
                color.b = Mathf.Clamp(138.5177312231f * Mathf.Log(temperatureClamped - 10) - 305.0447927307f, 0, 255);
            }
        }
        else
        {
            color.r += Mathf.Clamp(329.698727446f * Mathf.Pow(temperatureClamped - 60, -0.1332047592f), 0, 255);
            color.g += Mathf.Clamp(288.1221695283f * Mathf.Pow(temperatureClamped - 60, -0.0755148492f), 0, 255);
            color.b += 255;
        }
        color /= 255;
        return color;
    }


    /// <summary>
    /// Convert Cartesian coordiantes (xyz) into Polar coordinates (aid) (angle, inclination, distance)
    /// </summary>
    /// <param name="cartesian">The position in cartesian coordinates to be convereted</param>
    /// <param name="center">What the center of the polar coordinates should be.</param>
    /// <returns>A vector3 containing the polar coordinate. x = angle on horizontal plane (compas), y = inclination (angle above/below horizon), z = distance</returns>
    public static double[] CartesianToPolar(double[] cartesian, double[] center)
    {

        double[] polar = new double[3];
        if (cartesian.Length != 3 || center.Length != 3)
        {
            return polar;
        }

        double[] cartesianCorrected = new double[3] { cartesian[0] - center[0], cartesian[1] - center[1], cartesian[2] - center[3] };
        

        polar[2] = Math.Sqrt(Math.Pow(cartesianCorrected[0], 2) + Math.Pow(cartesianCorrected[1], 2) + Math.Pow(cartesianCorrected[2], 2));

        polar[0] = Math.Atan(cartesianCorrected[2] / cartesianCorrected[0]);
        if (cartesianCorrected[0] >= 0)
        {
            polar[0] += Math.PI;
        }
        if (double.IsNaN(polar[0]))
        {
            if (cartesianCorrected[0] >= 0)
            {
                polar[0] = 0;
            }
            else
            {
                polar[0] = Math.PI;
            }
        }
        polar[1] = Math.Acos(cartesianCorrected[1] / polar[2]);

        return polar;
    }
    /// <summary>
    /// Convert Cartesian coordiantes (xyz) into Polar coordinates (aid) (angle, inclination, distance)
    /// </summary>
    /// <param name="cartesian">The position in cartesian coordinates to be convereted</param>
    /// <param name="center">What the center of the polar coordinates should be.</param>
    /// <returns>A vector3 containing the polar coordinate. x = angle on horizontal plane (compas), y = inclination (angle above/below horizon), z = distance</returns>
    public static Vector3 CartesianToPolarFloat(double[] cartesian, double[] center)
    {

        Vector3 polar = new Vector3();
        if (cartesian.Length != 3 || center.Length != 3)
        {
            return polar;
        }

        double[] cartesianCorrected = new double[3] { cartesian[0] - center[0], cartesian[1] - center[1], cartesian[2] - center[2] };


        polar[2] = (float)Math.Sqrt(Math.Pow(cartesianCorrected[0], 2) + Math.Pow(cartesianCorrected[1], 2) + Math.Pow(cartesianCorrected[2], 2));

        polar[0] = (float)Math.Atan(cartesianCorrected[2] / cartesianCorrected[0]);
        if (cartesianCorrected[0] >= 0)
        {
            polar[0] += (float)Math.PI;
        }
        if (double.IsNaN(polar[0]))
        {
            if (cartesianCorrected[0] >= 0)
            {
                polar[0] = 0;
            }
            else
            {
                polar[0] = (float)Math.PI;
            }
        }
        polar[1] = (float)Math.Acos(cartesianCorrected[1] / polar[2]);

        return polar;
    }
}
