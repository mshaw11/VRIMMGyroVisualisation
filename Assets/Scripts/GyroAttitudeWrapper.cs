using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GyroAttitudeWrapper : MonoBehaviour
{

    public static List<GyroSample> WrapGyroValues(List<GyroSample> samples)
    {
        List<GyroSample> wrappedSamples = new List<GyroSample>();
        var xRollMultiplier = 0;
        var yRollMultiplier = 0;
        var zRollMultiplier = 0;

        for (int i = 0; i < samples.Count; i++)
        {
            var adjustedSample = new GyroSample(0.0f, 0.0f, 0.0f);
            if (i == 0)
            {
                adjustedSample.x = samples[i].x;
                adjustedSample.y = samples[i].y;
                adjustedSample.z = samples[i].z;
                wrappedSamples.Add(adjustedSample);
                continue;
            }

            var xDelta = samples[i].x - samples[i - 1].x;
            var yDelta = samples[i].y - samples[i - 1].y;
            var zDelta = samples[i].z - samples[i - 1].z;

            xRollMultiplier = AdjustForRoll(xDelta, xRollMultiplier);
            yRollMultiplier = AdjustForRoll(yDelta, yRollMultiplier);
            zRollMultiplier = AdjustForRoll(zDelta, zRollMultiplier);

            adjustedSample.x = samples[i].x + xRollMultiplier * 2;
            adjustedSample.y = samples[i].y + yRollMultiplier * 2;
            adjustedSample.z = samples[i].z + zRollMultiplier * 2;

            wrappedSamples.Add(adjustedSample);
        }
        return wrappedSamples;
    }

    private static int AdjustForRoll(float delta, int rollMultiplier)
    {
        //If roll occured
        if (Mathf.Abs(delta) > 1)
        {
            //If positive rollover
            if (delta > 0)
            {
                rollMultiplier--;
            }
            else
            {
                rollMultiplier++;
            }
        }
        return rollMultiplier;
    }
}
