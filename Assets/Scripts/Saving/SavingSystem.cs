using System.IO;
using System.Text;
using UnityEngine;



namespace RPG.Saving
{
    public class SavingSystem : MonoBehaviour
    {

        private string GetPathFromSaveFile(string saveFile)
        {
            return Path.Combine(Application.persistentDataPath, saveFile);
        }

        public void Save(string saveFile)
        {
            string path = GetPathFromSaveFile(saveFile);
            print("Saving to " + path);
            FileStream fstream = File.Open(path, FileMode.Create);

            string text = "Hello World";
            byte[] buffer = Encoding.UTF8.GetBytes(text);
            byte[] spanishBuffer = { 0x0d, 0xc2 ,0xa1, 0x48, 0x6f, 0x6c, 0x61, 0x20, 0x4d, 0x75, 0x6e, 0x64, 0x6f, 0x21 };
            fstream.Write(buffer, 0, buffer.Length);
            fstream.Write(spanishBuffer, 0, spanishBuffer.Length);
            fstream.Close();
        }

        public void Load(string saveFile)
        {
            print("Loading from " + GetPathFromSaveFile(saveFile));
        }
    }

}
