using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroOrientation : MonoBehaviour
{

    Gyroscope gyro;
    LineRenderer lineRenderer;
    public Color c1 = Color.yellow;
    public Color c2 = Color.red;

    private Vector3 defaultPosition = new Vector3(270, 0, 0);
    private Quaternion offset = new Quaternion();

    // Start is called before the first frame update
    void Start()
    {
        offset = Quaternion.identity;
        gyro = Input.gyro;
        gyro.enabled = true;
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.widthMultiplier = 0.2f;
        lineRenderer.positionCount = 2;

        // A simple 2 color gradient with a fixed alpha of 1.0f.
        float alpha = 1.0f;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(c1, 0.0f), new GradientColorKey(c2, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
        );
        lineRenderer.colorGradient = gradient;
    }


    //TODO: TRACK THE DIFF OF THE GYRO YOU DIV
    // Update is called once per frame
    void Update()
    {
        //Old attempt
        //transform.rotation = GyroToUnity(gyro.attitude) * rotationOffset * Quaternion.Euler(defaultPosition);

        // var gyroDelta = CalulateRotationDelta(GyroToUnity(gyro.attitude));
        //if angle is big enough
        // transform.rotation *= gyroDelta;

        transform.rotation = gyro.attitude * offset;
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, transform.position + transform.forward * 20);
        //Debug.DrawRay(transform.position, transform.forward * 20, Color.blue, 1.0f, false);
    }

    private Quaternion GyroToUnity(Quaternion q)
    {
        //x == x, y == z, z == y
        return new Quaternion(q.x, q.z, q.y, -q.w);
    }

    private Quaternion CalulateRotationDelta(Quaternion gyroRotation)
    {
        var objectRotation = transform.rotation;
        //qDelta = qTo * qFrom.inverse()
        var deltaRotation = gyroRotation * Quaternion.Inverse(objectRotation);
        return deltaRotation;
    }

    public void Recentre()
    {
        offset = Quaternion.FromToRotation(transform.rotation.eulerAngles, defaultPosition);
    }
}
