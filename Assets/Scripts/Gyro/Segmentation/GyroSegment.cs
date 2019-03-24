using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroSegment : MonoBehaviour
{
    public Vector3 position;
    public float radius;
    public string description;
    private Action myAction;

    public GyroSegment(Vector3 segment, float radius)
    {
        this.position = segment;
        this.radius = radius;
    }

    public void SetAction(Action callback)
    {
        myAction = callback;
    }

    public void Trigger()
    {
        myAction.Invoke();
    }



}
