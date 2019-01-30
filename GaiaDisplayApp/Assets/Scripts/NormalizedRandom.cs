﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Get a normalized random values. 
/// </summary>
public class NormalizedRandom : MonoBehaviour {

    /// <summary>
    /// Get a normalized random value
    /// </summary>
    /// <param name="r">Put a Random in</param>
    /// <returns>Get a random double</returns>
    public static double NextGaussianDouble(System.Random r)
    {
        double u, v, S;

        do
        {
            u = 2.0 * r.NextDouble() - 1.0;
            v = 2.0 * r.NextDouble() - 1.0;
            S = u * u + v * v;
        }
        while (S >= 1.0);

        double fac = Math.Sqrt(-2.0 * Math.Log(S) / S);
        return u * fac;
    }
}

