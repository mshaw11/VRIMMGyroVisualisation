using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Assets.Scripts
{
    public class LinearRecorder : MonoBehaviour
    {

        PersistentSave fileSystem = new PersistentSave();

        // Start is called before the first frame update
        void Start()
        {
            fileSystem.OpenFileToWrite(Application.persistentDataPath + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + ".txt");

        }

        // Update is called once per frame
        void Update()
        {
            foreach (var accel in Input.accelerationEvents)
            {
                fileSystem.WriteToFile(ToCsv(accel.acceleration.x, accel.acceleration.y, accel.acceleration.z, accel.deltaTime));
            }
        }

        public string ToCsv(float x, float y , float z)
        {
            return x.ToString() + ", " + y.ToString() + ", " + z.ToString() + "\n";
        }
        public string ToCsv(float x, float y, float z, float deltaTime)
        {
            return x.ToString() + ", " + y.ToString() + ", " + z.ToString() + ", " + deltaTime.ToString() + "\n";
        }
    }
}
