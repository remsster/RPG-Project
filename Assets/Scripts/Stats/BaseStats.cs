using System;
using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1, 99)]
        [SerializeField] private int startingLevel = 1;
        [SerializeField] private CharacterClass characterClass;
        [SerializeField] private Progression progression;
        [SerializeField] private GameObject levelUpParticleEffect;
        [SerializeField] private bool shouldUseModifiers = false;

        public event Action onLevelUp;

        private int curentLevel = 1;


        // ---------------------------------------------------------------------------------
        // Unity Engine Methods
        // ---------------------------------------------------------------------------------


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
                LevelUpEffect();
                onLevelUp();
            }
        }

        // ---------------------------------------------------------------------------------
        // Custom Methods
        // ---------------------------------------------------------------------------------

        // ---- Private ----

        private void LevelUpEffect()
        {
            Instantiate(levelUpParticleEffect, transform);
        }



        private float GetAdditiveModifier(Stat stat)
        {
            if (!shouldUseModifiers) return 0;
            float total = 0;
            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach (float modifier in provider.GetAdditiveModifiers(stat))
                {
                    total += modifier;
                }
            }
            return total;
        }

        private float GetPercentageModifier(Stat stat)
        {
            if (!shouldUseModifiers) return 0;
            float total = 0;
            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach (float modifier in provider.GetPercentageModifiers(stat))
                {
                    total += modifier;
                }
            }
            return total;
        }

        private float GetBaseStat(Stat stat) => progression.GetStat(stat, characterClass, GetLevel());
        

        // ---- Public ----

        public int GetLevel()
        {
            return curentLevel;
        }

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


        public float GetStat(Stat stat) => GetBaseStat(stat) + GetAdditiveModifier(stat) * (1 + GetPercentageModifier(stat) / 100);

        
    }
}
