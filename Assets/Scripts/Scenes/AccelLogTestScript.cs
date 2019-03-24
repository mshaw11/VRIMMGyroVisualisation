using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccelLogTestScript : MonoBehaviour
{

    PersistentSave rawAccelFile = new PersistentSave();
    private List<GyroSample> samples = new List<GyroSample>();
    PersistentSave wrappedOutput = new PersistentSave();
    GyroAttitudeWrapper wrapper = new GyroAttitudeWrapper();
    // Start is called before the first frame update
    void Start()
    {
        string fileName = "Assets/AccelLogs/files2019-03-23 063619.txt";
        rawAccelFile.OpenFileToRead(fileName);
        RunAccelScrub();
    }

    public void RunAccelScrub()
    {
        ReadAccelFile();
        Debug.Log("Count is " + samples.Count);
        // var wrappedSamples = GyroAttitudeWrapper.WrapGyroValues(samples);


        wrappedOutput.OpenFileToWrite("Assets/AccelLogs/output.txt");
        foreach (var rawSample in samples)
        {
            var adjustedSample = wrapper.WrapAttitudeValue(rawSample);
            wrappedOutput.WriteToFile(adjustedSample.ToCsv());
        }
        wrappedOutput.CloseFile();
        Debug.Log("Finished writing....");



        //wrappedOutput.OpenFileToWrite("Assets/AccelLogs/output.txt");
        //foreach (var sample in wrappedSamples)
        //{
        //    wrappedOutput.WriteToFile(sample.ToCsv());
        //}
        //wrappedOutput.CloseFile();
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
        samples.Add(new GyroSample(x, y, z));
    }


}
