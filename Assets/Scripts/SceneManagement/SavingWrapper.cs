using RPG.Saving;
using System;
using UnityEngine;


namespace RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        private const string DEFAULT_SAVE_FILE = "save";

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.L))
            {
                Load();
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                Save();
            }
        }

        public void Save()
        {
            GetComponent<SavingSystem>().Save(DEFAULT_SAVE_FILE);
        }

        public void Load()
        {
            // Call to the saving system
            GetComponent<SavingSystem>().Load(DEFAULT_SAVE_FILE);
        }
    }
}
