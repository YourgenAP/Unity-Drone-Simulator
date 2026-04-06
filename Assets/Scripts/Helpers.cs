using UnityEngine;
using System;

public class Helpers
{
    public float GaussRandom(float mu, float sigma)
    {
        float x1 = UnityEngine.Random.Range(0.0f, 1.0f);
        float x2 = UnityEngine.Random.Range(0.0f, 1.0f);

        float y1 = (float)(Math.Sqrt(-2.0 * Math.Log(x1)) * Math.Cos(2.0f * Math.PI * x2) * sigma + mu);
        return y1;
    }
}
