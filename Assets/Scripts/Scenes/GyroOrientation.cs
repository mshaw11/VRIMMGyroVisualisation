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

    [SerializeField]
    [NotNull]
    private Text playModeButtonText;

    [SerializeField]
    [NotNull]
    private Text segmentText;

    [SerializeField]
    private Button playModeButton;

    [SerializeField]
    private Button recordButton;

    //Gyro Stuff
    Gyroscope gyro;
    private List<GyroSample> samples = new List<GyroSample>();
    
    //Segmentation Stuff


    //Line Renderer Stuff
    LineRenderer lineRenderer;
    public Color c1 = Color.yellow;
    public Color c2 = Color.red;

    //Recording Stuff
    //PersistentSave fileSystem = new PersistentSave();
    private bool recording = false;

    //Music mode
    private bool musicPlaying = false;
    private List<GyroSegment> segments;
    private GyroAttitudeWrapper musicModeWrapper = new GyroAttitudeWrapper();
    private int lastSegmentIndex = -1;
    [SerializeField]
    MusicPlayer musicPlayer;

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

        //Always draw gyro and line
        var sample = new GyroSample(gyro.attitude.x, gyro.attitude.y, gyro.attitude.z);
        transform.rotation = gyro.attitude;
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, transform.position + transform.forward * 20);

        //We're either recording, music playing or neither
        if (recording)
        {
            samples.Add(sample);
            //fileSystem.WriteToFile(sample.ToCsv());
        }
        else if (musicPlaying)
        {
            var gyroWrapped = musicModeWrapper.WrapAttitudeValue(sample);
            var gyroWrappedAsVector = new Vector3(gyroWrapped.x, gyroWrapped.y, gyroWrapped.z);
            var closestSegmentIndex = SegmentFinder.FindIndexOfClosestSegment(segments, gyroWrappedAsVector);
            var insideRange = SegmentFinder.IsWithinEffectiveRange(segments[closestSegmentIndex], gyroWrappedAsVector);

            var currentSegment = segments[closestSegmentIndex];
            segmentText.text =
                "Segment: " + closestSegmentIndex +
                ". Inside range: " + insideRange.ToString() +
                ". Note is: " + currentSegment.description;

            if (lastSegmentIndex == -1 || closestSegmentIndex != lastSegmentIndex)
            {
                currentSegment.Trigger();
                lastSegmentIndex = closestSegmentIndex;
            }
        }
    }

    public void StartRecording()
    {
        recordButtonText.text = "Stop Recording";
        musicModeWrapper.Reset();
        samples.Clear();
        //fileSystem.OpenFileToWrite(Application.persistentDataPath +  DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + ".txt");
        playModeButton.interactable = false;
    }

    public void StopRecording()
    {
        recordButtonText.text = "Record";
        //fileSystem.CloseFile();
        ProcessAndSegmentGyro();
        playModeButton.interactable = true;
    }

    public void ProcessAndSegmentGyro()
    {
        var wrappedSamples = GyroAttitudeWrapper.WrapAttitudeValues(samples);
        GyroLinearSegmentation segmentation = new GyroLinearSegmentation(wrappedSamples);
        segmentation.CalculateSegments(7);
        segments = segmentation.GetSegments();
    }

    public void EnterMusicMode()
    {
        recordButton.interactable = false;
        playModeButtonText.text = "Stop playing music";
        SetupSegmentsToPlayMusic();
    }

    public void LeaveMusicMode()
    {
        recordButton.interactable = true;
        playModeButtonText.text = "Start playing music";
        segmentText.text = "Not is music mode...";
        ClearSegmentsOfDescriptionAndMusic();
    }

    public void SetupSegmentsToPlayMusic()
    {
        if (segments.Count == 7)
        {
            segments[0].description = "A";
            segments[0].SetAction(new Action(() => musicPlayer.Play(MusicPlayer.Notes.A)));
            segments[1].description = "B";
            segments[1].SetAction(new Action(() => musicPlayer.Play(MusicPlayer.Notes.B)));
            segments[2].description = "C";
            segments[2].SetAction(new Action(() => musicPlayer.Play(MusicPlayer.Notes.C)));
            segments[3].description = "D";
            segments[3].SetAction(new Action(() => musicPlayer.Play(MusicPlayer.Notes.D)));
            segments[4].description = "E";
            segments[4].SetAction(new Action(() => musicPlayer.Play(MusicPlayer.Notes.E)));
            segments[5].description = "F";
            segments[5].SetAction(new Action(() => musicPlayer.Play(MusicPlayer.Notes.F)));
            segments[6].description = "G";
            segments[6].SetAction(new Action(() => musicPlayer.Play(MusicPlayer.Notes.G)));
        }
        else
        {
            Debug.LogError("Wrong number of segments for a scale, should be 7 but count is: " + segments.Count);
        }
    }

    public void ClearSegmentsOfDescriptionAndMusic()
    {
        foreach (var segment in segments)
        {
            segment.description = "";
            segment.SetAction(null);
        }
    }

    public void Recentre()
    {
        musicModeWrapper.Reset();
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

    public void ToggleMusicMode()
    {
        if (!recording)
        {
            musicPlaying = !musicPlaying;
            if (musicPlaying)
            {
                EnterMusicMode();
            }
            else
            {
                LeaveMusicMode();
            }
        }
    }
}
