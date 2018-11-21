using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Star
{

    public struct BasicStar
    {
        public Vector3 coordinates;
        public Vector4 colour;
        public float magnitude;
    }

    

    public string designation;
    public double ref_epoch;
    public double ra;
    public double dec;
    public double distance;
    public double x;
    public double y;
    public double z;
    public double pmra;
    public double pmdec;
    public double astrometric_pseudo_colour;
    public float phot_g_mean_mag;
    public double radial_velocity;

    public List<double> values;

    public static explicit operator BasicStar(Star s)
    {
        BasicStar bs = new BasicStar();
        bs.coordinates = CartesianToPolarFloat(new double[] { s.x, s.y, s.z }, StarRenderer.Instance.cameraPosition);
        bs.colour = new Vector4(1, 1, 1, 1);
        bs.magnitude = s.Magnitude;
        return bs;
    }

    /* public double Date { get { return values[0]; } set { values[0] = value; } }
     public double RightAccension { get { return values[1]; } set { values[1] = value; } }
     public double Inclination { get { return values[3]; } set { values[3] = value; } }
     public double Distance { get { return values[5]; } set { values[5] = value; } }
     //public double Radius { get { return values[0]; } set { values[0] = value; } }

     public double ProperMotionRightAccension { get { return values[7]; } set { values[7] = value; } }
     public double ProperMotionInclination { get { return values[9]; } set { values[9] = value; } }
     public double RadialVelocity { get { return values[14]; } set { values[14] = value; } }

     public double Error_RA { get { return values[2]; } set { values[2] = value; } }
     public double Error_I { get { return values[4]; } set { values[4] = value; } }
     public double Error_RV { get { return values[15]; } set { values[15] = value; } }
     public double Error_D { get { return values[6]; } set { values[6] = value; } }

     public double Error_pmRA { get { return values[8]; } set { values[8] = value; } }
     public double Error_pmI { get { return values[10]; } set { values[10] = value; } }

     public float Magnitude { get { return (float)values[13]; } set { values[13] = value; } }
     */
    public double Date { get { return ref_epoch; } set { ref_epoch = value; } }
    public double RightAccension { get { return ra; } set { ra = value; } }
    public double Inclination { get { return dec; } set { dec = value; } }
    public double Distance { get { return distance; } set { distance = value; } }
    //public double Radius { get { return values[0]; } set { values[0] = value; } }

    public double ProperMotionRightAccension { get { return pmra; } set { pmra = value; } }
    public double ProperMotionInclination { get { return pmdec; } set { pmdec = value; } }
    public double RadialVelocity { get { return radial_velocity; } set { radial_velocity = value; } }
    
    public float Magnitude { get { return phot_g_mean_mag; } set { phot_g_mean_mag = value; } }

    public Star(double ra, double i, double d, double r, double pmRA, double pmI, float m)
    {
        if (values == null)
        {
            values = new List<double>();
        }
        RightAccension = ra;
        Inclination = i;
        Distance = d;
        ProperMotionRightAccension = pmRA;
        ProperMotionInclination = pmI;
        //radius = r;
        Magnitude = m;
    }

    public Color ColorTemperature(float t)
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
        polar[1] = Math.Asin(cartesianCorrected[1] / polar[2]);

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

        double[] cartesianCorrected = new double[3] { cartesian[0] - center[0], cartesian[1] - center[1], cartesian[2] - center[3] };


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
        polar[1] = (float)Math.Asin(cartesianCorrected[1] / polar[2]);

        return polar;
    }
}
