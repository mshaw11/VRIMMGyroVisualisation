using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class PersistentSave
    {
        FileStream file;
        StreamReader streamReader;

        public void OpenFileToWrite(string path)
        {
            if (file != null)
            {
                file.Close();
            }
            if (File.Exists(path)) file = File.OpenWrite(path);
            else file = File.Create(path);
            Debug.Log("File being created at: " + path);
        }

        public void WriteToFile(string textToAppend)
        {
            var stringAsBytes = Encoding.ASCII.GetBytes(textToAppend);
            file.Write(stringAsBytes
                ,
                0,
                stringAsBytes.Length);
        }

        public void CloseFile()
        {
            if (file != null)
            {
                file.Close();
            }

            if (streamReader != null)
            {
                streamReader.Close();
            }
        }

        public void OpenFileToRead(string filename)
        {
            if (file != null)
            {
                file.Close();
            }
            string destination = filename;
            Debug.Log(destination);
            if (File.Exists(destination)) streamReader = new StreamReader(destination);
            else
            {
                Debug.LogError("File not found");
                return;
            }
        }
 
        public string ReadLine()
        {
            var line = streamReader.ReadLine();
            return line;
        }
    }
}
