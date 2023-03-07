using System.Collections;
using UnityEngine;

using RPG.Saving;

namespace RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        const string DEFAULT_SAVE_FILE = "save";
        [SerializeField] float fadeInTime = 0.2f;

        private IEnumerator Start()
        {
            Fader fader = FindObjectOfType<Fader>();
            fader.FadeOutImmediate();
            yield return GetComponent<SavingSystem>().LoadLastScene(DEFAULT_SAVE_FILE);
            yield return fader.FadeIn(fadeInTime);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                Save();
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                Load();
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                Delete();
            }

        }

        private void Delete()
        {
            GetComponent<SavingSystem>().Delete(DEFAULT_SAVE_FILE);
        }

        public void Load()
        {
            GetComponent<SavingSystem>().Load(DEFAULT_SAVE_FILE);
        }

        public void Save()
        {
            GetComponent<SavingSystem>().Save(DEFAULT_SAVE_FILE);
        }
    }
}
