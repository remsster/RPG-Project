using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;


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

        private Dictionary<string, object> CaptureState()
        {
            Dictionary<string, object> state = new Dictionary<string, object>();
           
            foreach(SaveableEntity saveable in FindObjectsOfType<SaveableEntity>())
            {
                state[saveable.GetUniqueIdentifier()] =  saveable.CaptureState();
            }
            return state; 
        }

        private void RestoreState(Dictionary<string, object> state)
        {
            foreach (SaveableEntity saveable in FindObjectsOfType<SaveableEntity>())
            {
                saveable.RestoreState(state[saveable.GetUniqueIdentifier()]);
            }

        }

        private void SaveFile(string saveFile, object state)
        {
            string path = GetPathFromSaveFile(saveFile);

            using (FileStream fstream = File.Open(path, FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fstream, state);
            }
        }

        private Dictionary<string,object> LoadFile(string saveFile)
        {
            string path = GetPathFromSaveFile(saveFile);
            using (FileStream fstream = File.Open(path, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                return (Dictionary<string,object>)formatter.Deserialize(fstream);
            }
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
            SaveFile(saveFile, CaptureState());
        }

        public void Load(string saveFile)
        {
            RestoreState(LoadFile(saveFile));
        }

        
    }

}
