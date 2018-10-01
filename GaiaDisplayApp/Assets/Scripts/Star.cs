using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Star
{

    public double rightAccension;
    public double inclination;
    public double distance;
    public double radius;

    public double properMotionRightAccension;
    public double properMotionInclination;

    public double error_RA;
    public double error_I;
    public double error_D;

    public double error_pmRA;
    public double error_pmI;

    public float temperature;
    public float magnitude;

    public Color temperatureColor;


    public Star(double ra, double i, double d, double r, double pmRA, double pmI, double eRA, double eI, double eD, double ePMRA, double ePMI, float t, float m)
    {
        this.rightAccension = ra;
        this.inclination = i;
        this.distance = d;
        this.properMotionRightAccension = pmRA;
        this.properMotionInclination = pmI;
        this.radius = r;
        this.error_RA = eRA;
        this.error_I = eI;
        this.error_D = eD;
        this.error_pmRA = ePMRA;
        this.error_pmI = ePMI;
        this.temperature = t;
        this.magnitude = m;

        this.temperatureColor = ColorTemperature(t);
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
