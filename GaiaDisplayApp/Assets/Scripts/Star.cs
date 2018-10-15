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
    public double ra_error;
    public double dec;
    public double dec_error;
    public double distance;
    public double distance_error;
    public double pmra;
    public double pmra_error;
    public double pmdec;
    public double pmdec_error;
    public double astrometric_pseudo_colour;
    public double astrometric_pseudo_colour_error;
    public float phot_g_mean_mag;
    public double radial_velocity;
    public double radial_velocity_error;
    public double l;
    public double b;
    public double ecl_long;
    public double ecl_lat;

    public List<double> values;

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

    public double Error_RA { get { return ra_error; } set { ra_error = value; } }
    public double Error_I { get { return dec_error; } set { dec_error = value; } }
    public double Error_RV { get { return radial_velocity_error; } set { radial_velocity_error = value; } }
    public double Error_D { get { return distance_error; } set { distance_error = value; } }

    public double Error_pmRA { get { return pmra_error; } set { pmra_error = value; } }
    public double Error_pmI { get { return pmdec_error; } set { pmdec_error = value; } }

    public float Magnitude { get { return phot_g_mean_mag; } set { phot_g_mean_mag = value; } }

    public Star(double ra, double i, double d, double r, double pmRA, double pmI, double eRA, double eI, double eD, double ePMRA, double ePMI, float m)
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
        Error_RA = eRA;
        Error_I = eI;
        Error_D = eD;
        Error_pmRA = ePMRA;
        Error_pmI = ePMI;
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
}
