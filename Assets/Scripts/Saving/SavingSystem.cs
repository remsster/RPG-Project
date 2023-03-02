using System;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Saving
{
    public class SavingSystem : MonoBehaviour
    {

        // ---------------------------------------------------------------------------------
        // Custom Methods
        // ---------------------------------------------------------------------------------

        // ---- Private ----

        private string GetPathFromSaveFile(string saveFile)
        {
            return Path.Combine(Application.persistentDataPath, saveFile);
        }

        private byte[] SerializeVector(Vector3 vector)
        {
            byte[] vectorBytes = new byte[3 * 4];
            BitConverter.GetBytes(vector.x).CopyTo(vectorBytes, 0);
            BitConverter.GetBytes(vector.y).CopyTo(vectorBytes, 4);
            BitConverter.GetBytes(vector.z).CopyTo(vectorBytes, 8);
            return vectorBytes;
        }

        private Vector3 DeserializeVector3(byte[] buffer)
        {
            Vector3 result = new Vector3();
            result.x = BitConverter.ToSingle(buffer, 0);
            result.y = BitConverter.ToSingle(buffer, 4);
            result.z = BitConverter.ToSingle(buffer, 8);
            return result;
        }

        private Transform GetPlayerTransform()
        {
            return GameObject.FindWithTag("Player").transform;
        }

        // ---- Public ----

        public void Delete(string saveFile)
        {
            string path = GetPathFromSaveFile(saveFile);
            if (File.Exists(path))
            {
                print("Deleting file");
                File.Delete(path);
            }
            
        }

        public void Save(string saveFile)
        {
            string path = GetPathFromSaveFile(saveFile);

            using(FileStream fstream = File.Open(path, FileMode.Create))
            {
                Transform playerTransform = GetPlayerTransform();
                byte[] buffer = SerializeVector(playerTransform.position);
                fstream.Write(buffer, 0, buffer.Length);
            }
        }

        public void Load(string saveFile)
        {
            string path =  GetPathFromSaveFile(saveFile);

            using (FileStream fstream = File.Open(path, FileMode.Open))
            {
                byte[] buffer = new byte[fstream.Length];
                fstream.Read(buffer, 0, buffer.Length);
                Transform playerTransform = GetPlayerTransform();
                playerTransform.position = DeserializeVector3(buffer);
            }
        }
    }

}
