using System.Collections;
using RPG.Saving;
using UnityEngine;


namespace RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {

        [SerializeField] private float fadeInTime = 2f;

        private const string DEFAULT_SAVE_FILE = "save";
        private Fader fader;

        private void Awake()
        {
            fader = FindObjectOfType<Fader>();
        }

        private IEnumerator Start()
        {
            fader.FadeOutImmediate();
            yield return GetComponent<SavingSystem>().LoadLastScene(DEFAULT_SAVE_FILE);
            yield return fader.FadeIn(fadeInTime);
        }

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
