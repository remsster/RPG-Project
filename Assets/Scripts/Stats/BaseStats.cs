
using UnityEngine;

namespace RPG.Stats
{ 
    public class BaseStats : MonoBehaviour
    {
        [Range(1,99)]
        [SerializeField] private int startingLevel = 1;
        [SerializeField] private CharacterClass characterClass;
        [SerializeField] private Progression progression;

        private int curentLevel = 0;

        private void Start()
        {
            curentLevel = CalculateLevel();
            Experience experience = GetComponent<Experience>();
            if (experience != null)
            {
                experience.onExperienceGained += UpdateLevel;
            }
        }

        private void UpdateLevel()
        {
            int newLevel = CalculateLevel();
            if (newLevel > curentLevel)
            {
                curentLevel = newLevel;
                print("BaseStat: Leveled Up!");
            }
        }

        private int GetLevel()
        {
            if (curentLevel < 1) curentLevel = CalculateLevel();
            return curentLevel;
        }
        

        public float GetStat(Stat stat) => progression.GetStat(stat, characterClass, GetLevel());


        public int CalculateLevel()
        {
            Experience experience = GetComponent<Experience>();
            if (experience == null) return startingLevel;
            float currentXP = experience.ExperiencePoints;
            
            int penultimateLevel = progression.GetLevels(Stat.ExperienceToLevelUp, characterClass);
            for (int level = 1; level <= penultimateLevel; level++)
            {
                float xpToLevelUp = progression.GetStat(Stat.ExperienceToLevelUp, characterClass, level);
                if (xpToLevelUp > currentXP)
                {
                    return level;
                }
            }
            return penultimateLevel + 1;
        }
    }
}
