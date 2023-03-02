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

        private object CaptureState()
        {
            Dictionary<string, object> state = new Dictionary<string, object>();
            int x = 0;
            foreach(SaveableEntity saveable in FindObjectsOfType<SaveableEntity>())
            {
                state[saveable.GetUniqueIdentifier()] =  saveable.CaptureState();
                x++;
            }
            print("Captured " + x + " states");
            return state; 
        }

        private void RestoreState(object state)
        {
            Dictionary<string, object> stateDict = (Dictionary<string, object>)state;
            foreach (SaveableEntity saveable in FindObjectsOfType<SaveableEntity>())
            {
                saveable.RestoreState(stateDict[saveable.GetUniqueIdentifier()]);
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
            string path = GetPathFromSaveFile(saveFile);

            using(FileStream fstream = File.Open(path, FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fstream,  CaptureState());                
            }
        }

        public void Load(string saveFile)
        {
            string path =  GetPathFromSaveFile(saveFile);

            using (FileStream fstream = File.Open(path, FileMode.Open))
            {
                
                BinaryFormatter formatter = new BinaryFormatter();
                RestoreState(formatter.Deserialize(fstream));
                
            }
        }        
    }

}
