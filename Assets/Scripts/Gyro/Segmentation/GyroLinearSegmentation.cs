using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroLinearSegmentation
{
    private List<GyroSample> _samples;
    private List<GyroSegment> _segments;


    public GyroLinearSegmentation(List<GyroSample> samples)
    {
        this._samples = samples;
    }

    public void CalculateSegments(int segmentCount)
    {

        _segments = new List<GyroSegment>();
        var count = _samples.Count;

        Vector3 firstSample = new Vector3(_samples[0].x, _samples[0].y, _samples[0].z);
        Vector3 lastSample = new Vector3(_samples[count].x, _samples[count].y, _samples[count].z);

        var line = lastSample - firstSample;
        var lineNormalised = line.normalized;
        var lineLength = line.magnitude;
        var scale = lineLength / segmentCount;
        var radius = scale / 2;
        for (int i = 0; i < segmentCount; i++)
        {
            Vector3 pointOnLine = firstSample + ((scale * i) * lineNormalised);
            _segments.Add(new GyroSegment(pointOnLine, radius));
        }
    }

    public List<GyroSegment> GetSegments()
    {
        return _segments;
    }


}
