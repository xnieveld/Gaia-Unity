using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NormalizedRandom : MonoBehaviour {

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

