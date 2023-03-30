using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        private void CaptureState(Dictionary<string, object> state)
        {
            foreach(SaveableEntity saveable in FindObjectsOfType<SaveableEntity>())
            {
                state[saveable.GetUniqueIdentifier()] =  saveable.CaptureState();
            }
            state["lastSceneBuildIndex"] = SceneManager.GetActiveScene().buildIndex;
        }

        private void RestoreState(Dictionary<string, object> state)
        {
            foreach (SaveableEntity saveable in FindObjectsOfType<SaveableEntity>())
            {
                string id = saveable.GetUniqueIdentifier();
                if(state.ContainsKey(id))
                {
                    saveable.RestoreState(state[id]);
                }
            }

        }


        /// <summary>
        /// All this does is serialize data
        /// </summary>
        /// <param name="saveFile">the file thay the data is saved to</param>
        /// <param name="state">captured game state</param>
        private void SaveFile(string saveFile, object state)
        {
            string path = GetPathFromSaveFile(saveFile);

            using (FileStream fstream = File.Open(path, FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fstream, state);
            }
        }
        
        /// <summary>
        /// All this does is dserialize data
        /// </summary>
        /// <param name="saveFile">The file to load the data from</param>
        /// <returns>deserialized data that can update game state</returns>
        private Dictionary<string,object> LoadFile(string saveFile)
        {
            string path = GetPathFromSaveFile(saveFile);
            if (!File.Exists(path))
            {
                return new Dictionary<string, object>();
            }
            using (FileStream fstream = File.Open(path, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                return (Dictionary<string,object>)formatter.Deserialize(fstream);
            }
        }

        // ---- Public ----

        public IEnumerator LoadLastScene(string saveFile)
        {
            Dictionary<string, object> state = LoadFile(saveFile);
            int buildIndex = 0;
            if (state.ContainsKey("lastSceneBuildIndex"))
            {
                buildIndex = (int)state["lastSceneBuildIndex"];
                if (buildIndex != SceneManager.GetActiveScene().buildIndex)
                {
                    buildIndex = (int)state["lastSceneBuildIndex"];
                }
            }
            yield return SceneManager.LoadSceneAsync(buildIndex);
            RestoreState(state);

        }

        public void Delete(string saveFile)
        {
            string path = GetPathFromSaveFile(saveFile);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        public void Save(string saveFile)
        {
            Dictionary<string,object> state = LoadFile(saveFile);
            CaptureState(state);
            SaveFile(saveFile, state);
        }

        public void Load(string saveFile)
        {
            RestoreState(LoadFile(saveFile));
        }

        
    }

}
