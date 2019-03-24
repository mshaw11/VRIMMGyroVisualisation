using Assets.Scripts;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * Class to form the foundation of a VRIMM gyroscope test app.
 * The app will dynamically segment a gyroscope recording into points and then use this to play notes and chords. 
 */

public class GyroOrientation : MonoBehaviour
{

    //UI Stuff
    [SerializeField]
    [NotNull]
    private Text recordButtonText;

    //Gyro Stuff
    Gyroscope gyro;
    private List<GyroSample> samples = new List<GyroSample>();


    //Line Renderer Stuff
    LineRenderer lineRenderer;
    public Color c1 = Color.yellow;
    public Color c2 = Color.red;

    //Recording Stuff
    PersistentSave fileSystem = new PersistentSave();
    private bool recording = false;


    // Start is called before the first frame update
    void Start()
    {
        gyro = Input.gyro;
        gyro.enabled = true;
        InitLine();

    }

    private void InitLine()
    {
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

    void Update()
    {
        transform.rotation = gyro.attitude;

        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, transform.position + transform.forward * 20);

        if (recording)
        {
            var sample = new GyroSample(gyro.attitude.x, gyro.attitude.y, gyro.attitude.z);
            samples.Add(sample);
            fileSystem.WriteToFile(sample.ToCsv());
        }
    }

    public void StartRecording()
    {
        recordButtonText.text = "Stop Recording";
        samples.Clear();
        fileSystem.OpenFileToWrite(Application.persistentDataPath +  DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + ".txt");
    }

    public void StopRecording()
    {
        recordButtonText.text = "Record";
        fileSystem.CloseFile();
    }

    public void Recentre()
    {
       //TODO: Recentre button
    }

    public void ToggleRecord()
    {
        recording = !recording;
        if (recording)
        {
            StartRecording();
        }
        else
        {
            StopRecording();
        }

    }
}
