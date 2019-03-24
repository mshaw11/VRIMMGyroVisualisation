using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 *  Utility class for dealing with rollover during gyroscope tracking.
 *  Creates smooth coordinates based on the number of times the gyroscope has been turned over.
 *  Don't leave home without it if you're going to do mapping based on gyro.
 *  Should ideally be converted to Vector 3
 *  
 *  Author: Matthew Shaw
 * */
public class GyroAttitudeWrapper : MonoBehaviour
{

    private int _xRollMultiplier = 0;
    private int _yRollMultiplier = 0;
    private int _zRollMultiplier = 0;
    private GyroSample _previousSample;

    /* 
     *  Returns the wrapped version of the current sample. Counts number of rolls the gyroscope has done.
     *  Reset internal state tracking by calling Reset()
     */
    public GyroSample WrapAttitudeValue(GyroSample newSample)
    {
        if (_previousSample == null)
        {
            //Can't tell if we rolled if this is the first sample
            _previousSample = newSample;
            return newSample;
        }
        var adjustedSample = Wrap(newSample);
        //Wrap then before returning, adjust previousSample
        _previousSample = newSample;
        return adjustedSample;
    }

    public void Reset()
    {
        _previousSample = null;
        _xRollMultiplier = 0;
        _yRollMultiplier = 0;
        _zRollMultiplier = 0;
    }

    private GyroSample Wrap(GyroSample newSample)
    {
        var adjustedSample = new GyroSample(0.0f, 0.0f, 0.0f);

        var xDelta = newSample.x - _previousSample.x;
        var yDelta = newSample.y - _previousSample.y;
        var zDelta = newSample.z - _previousSample.z;

        _xRollMultiplier = CheckAndAdjustForRoll(xDelta, _xRollMultiplier);
        _yRollMultiplier = CheckAndAdjustForRoll(yDelta, _yRollMultiplier);
        _zRollMultiplier = CheckAndAdjustForRoll(zDelta, _zRollMultiplier);

        adjustedSample.x = newSample.x + _xRollMultiplier * 2;
        adjustedSample.y = newSample.y + _yRollMultiplier * 2;
        adjustedSample.z = newSample.z + _zRollMultiplier * 2;

        return adjustedSample;
    }

    private static int CheckAndAdjustForRoll(float delta, int rollMultiplier)
    {
        //If roll occured
        if (Mathf.Abs(delta) > 1)
        {
            //If negative rollover occured
            if (delta > 0)
            {
                rollMultiplier--;
            }
            else
            {
                //positive rollover occured
                rollMultiplier++;
            }
        }
        return rollMultiplier;
    }

    /**
     * For a one shot wrapping of a list of gyro values
     * **/
    public static List<GyroSample> WrapAttitudeValues(List<GyroSample> samples)
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

            xRollMultiplier = CheckAndAdjustForRoll(xDelta, xRollMultiplier);
            yRollMultiplier = CheckAndAdjustForRoll(yDelta, yRollMultiplier);
            zRollMultiplier = CheckAndAdjustForRoll(zDelta, zRollMultiplier);

            adjustedSample.x = samples[i].x + xRollMultiplier * 2;
            adjustedSample.y = samples[i].y + yRollMultiplier * 2;
            adjustedSample.z = samples[i].z + zRollMultiplier * 2;

            wrappedSamples.Add(adjustedSample);
        }
        return wrappedSamples;
    }
}
