using RPG.Saving;
using UnityEngine;

namespace RPG.Attributes
{
    public class Experience: MonoBehaviour, ISaveable
    {
        [SerializeField] private float experiencePoints = 0;


        // ---- Public ----

        public void GainExperience(float experience)
        {
            experiencePoints += experience;
        }

        public void RestoreState(object state)
        {
            experiencePoints = (float)state;
        }

        // ---- ISaveable ----

        public object CaptureState()
        {
            return experiencePoints;
        }
    }
}
