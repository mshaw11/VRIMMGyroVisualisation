﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class GyroSample
{
    public float x, y, z;
    public GyroSample(float x, float y, float z)
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
