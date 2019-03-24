using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroSegment : MonoBehaviour
{
    public Vector3 position;
    public float radius;
    public GyroSegment(Vector3 segment, float radius)
    {
        this.position = segment;
        this.radius = radius;
    }

}
