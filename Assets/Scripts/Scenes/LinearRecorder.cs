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
            fileSystem.WriteToFile(ToCsv(Input.acceleration.x, Input.acceleration.y, Input.acceleration.z));
        }

        public string ToCsv(float x, float y , float z)
        {
            return x.ToString() + ", " + y.ToString() + ", " + z.ToString() + "\n";
        }
    }
}
