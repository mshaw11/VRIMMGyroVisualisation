using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class LinearLogTestScript : MonoBehaviour
{

    PersistentSave rawAccelFile = new PersistentSave();
    PersistentSave outputFile = new PersistentSave();
    PersistentSave gapsOutputFile = new PersistentSave();
    private List<LinearSample> samples = new List<LinearSample>();
    private List<int> gaps = new List<int>();

    // Start is called before the first frame update
    void Start()
    {
        string fileName = "Assets/AccelLogs/files2019-10-22 090454.txt";
        string outputFileName = "Assets/AccelLogs/mags.txt";
        string gapsOutputFileName = "Assets/AccelLogs/gaps.txt";
        rawAccelFile.OpenFileToRead(fileName);
        outputFile.OpenFileToWrite(outputFileName);
        gapsOutputFile.OpenFileToWrite(gapsOutputFileName);
        RunAccelScrub();
    }

    public void RunAccelScrub()
    {
        ReadAccelFile();
        Debug.Log("Count is " + samples.Count);


        foreach (var rawSample in samples)
        {
            var accel = new Vector3(rawSample.x, rawSample.y, rawSample.z);
            //outputFile.WriteToFile(accel.magnitude.ToString() + "\n");
        }
        var indexOfLastPeak = 0;
        for (int i = 0; i < samples.Count; i++)
        {
            var rawSample = samples[i];
            var accel = new Vector3(rawSample.x, rawSample.y, rawSample.z);
            if (i > 1000 && i < 2000)
            {

                if (accel.magnitude > 2)
                {
                    if (indexOfLastPeak == 0)
                    {
                        indexOfLastPeak = i;
                    }
                    var gap = i - indexOfLastPeak;
                    indexOfLastPeak = i;
                    gaps.Add(gap);
                    gapsOutputFile.WriteToFile(gap + "\n");
                }
            }
        }
        AnalyseGaps();
        // var wrappedSamples = GyroAttitudeWrapper.WrapGyroValues(samples);


        //wrappedOutput.OpenFileToWrite("Assets/AccelLogs/output.txt");
        //foreach (var rawSample in samples)
        //{
        //    var adjustedSample = wrapper.WrapAttitudeValue(rawSample);
        //    wrappedOutput.WriteToFile(adjustedSample.ToCsv());
        //}
        //wrappedOutput.CloseFile();
        //Debug.Log("Finished writing....");



        //wrappedOutput.OpenFileToWrite("Assets/AccelLogs/output.txt");
        //foreach (var sample in wrappedSamples)
        //{
        //    wrappedOutput.WriteToFile(sample.ToCsv());
        //}
        //wrappedOutput.CloseFile();
    }

    public void AnalyseGaps()
    {
        var average = gaps.Average();
        var sd = Math.Sqrt(gaps.Average(v => Math.Pow(v - average, 2)));
        Debug.Log("Average gap is: " + average);
        Debug.Log("STD is: " + sd);
    }
    public void ReadAccelFile()
    {
        var line = rawAccelFile.ReadLine();
        while (line != null)
        {
            ParseAccelLine(line);
            line = rawAccelFile.ReadLine();
        }
        rawAccelFile.CloseFile();
    }
    public void ParseAccelLine(string accelLine)
    {
        string[] accelValues = accelLine.Split(',');
        float x = float.Parse(accelValues[0]);
        float y = float.Parse(accelValues[1]);
        float z = float.Parse(accelValues[2]);
        samples.Add(new LinearSample(x, y, z));
    }


}
