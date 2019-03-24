using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroSegment : MonoBehaviour
{
    public Vector3 segment;
    public float radius;
    public GyroSegment(Vector3 segment, float radius)
    {
        this.segment = segment;
        this.radius = radius;
    }

}
