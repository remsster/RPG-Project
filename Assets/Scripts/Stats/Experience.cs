using RPG.Saving;
using UnityEngine;

namespace RPG.Stats
{
    public class Experience: MonoBehaviour, ISaveable
    {
        [SerializeField] private float experiencePoints = 0;

        public float ExperiencePoints => experiencePoints;

        // ---- Public ----

        public void GainExperience(float experience)
        {
            experiencePoints += experience;
        }

        // ---- ISaveable ----

        public void RestoreState(object state)
        {
            experiencePoints = (float)state;
        }


        public object CaptureState()
        {
            return experiencePoints;
        }
    }
}
