using System;
using UnityEngine;

using RPG.Saving;

namespace RPG.Stats
{
    public class Experience: MonoBehaviour, ISaveable
    {
        [SerializeField] private float experiencePoints = 0;

        //public delegate void ExperienceGainedDelegate();
        public event Action onExperienceGained;

        public float ExperiencePoints => experiencePoints;

        // ---- Public ----

        public void GainExperience(float experience)
        {
            experiencePoints += experience;
            onExperienceGained();
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
