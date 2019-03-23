using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GyroAttitudeWrapper : MonoBehaviour
{

    List<GyroSample> wrappedSamples;

    public List<GyroSample> WrapGyroValues(List<GyroSample> samples)
    {

        wrappedSamples = new List<GyroSample>();
        var xRolls = 0;
        var yRolls = 0;
        var zRolls = 0;

        for (int i = 0; i < samples.Count; i++)
        {
            var adjustedSample = new GyroSample(0.0f, 0.0f, 0.0f);
            if (i == 0)
            {
                continue;
            }

            var xDelta = samples[i].x - samples[i - 1].x;
            var yDelta = samples[i].y - samples[i - 1].y;
            var zDelta = samples[i].z - samples[i - 1].z;

            xRolls = AdjustForRoll(xDelta, xRolls);
            yRolls = AdjustForRoll(xDelta, yRolls);
            zRolls = AdjustForRoll(xDelta, zRolls);

            adjustedSample.x = samples[i].x + xRolls * 2;
            adjustedSample.y = samples[i].y + yRolls * 2;
            adjustedSample.z = samples[i].z + zRolls * 2;
            wrappedSamples.Add(adjustedSample);
        }
        return wrappedSamples;
    }

    private int AdjustForRoll(float delta, int rollCount)
    {
        //If roll occured
        if (Mathf.Abs(delta) > 1)
        {
            //If positive roll
            if (delta > 0)
            {
                rollCount++;
            }
            else
            {
                rollCount--;
            }
        }
        return rollCount;
    }
}
