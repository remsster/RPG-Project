using System.IO;
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
            print("Saving to " + GetPathFromSaveFile(saveFile));
        }

        public void Load(string saveFile)
        {
            print("Loading from " + GetPathFromSaveFile(saveFile));
        }
    }

}
