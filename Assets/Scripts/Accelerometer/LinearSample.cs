using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearSample
{
    public float x, y, z;
    public LinearSample(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public string ToCsv()
    {
        return x.ToString() + ", " + y.ToString() + ", " + z.ToString() + "\n";
    }
}
