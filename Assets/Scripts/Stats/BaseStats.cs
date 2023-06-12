using System;
using UnityEngine;

using GameDevTV.Utils;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        // The player always starts from level 1 as it has the experience component
        // This currently cannot be adjusted in the editor for the player only
        [Range(1, 99)]
        [SerializeField] private int startingLevel = 1;
        [SerializeField] private CharacterClass characterClass;
        [SerializeField] private Progression progression;
        [SerializeField] private GameObject levelUpParticleEffect;
        [SerializeField] private bool shouldUseModifiers = false;

        public event Action onLevelUp;

        private LazyValue<int> curentLevel;

        Experience experience;


        // ---------------------------------------------------------------------------------
        // Unity Engine Methods
        // ---------------------------------------------------------------------------------

        private void Awake()
        {
            experience = GetComponent<Experience>();
            curentLevel = new LazyValue<int>(CalculateLevel);
        }


        private void Start()
        {
            curentLevel.ForceInit();
        }

        private void OnEnable()
        {
            if (experience != null)
            {
                experience.onExperienceGained += UpdateLevel;
            }
        }

        private void OnDisable()
        {
            if (experience != null)
            {
                experience.onExperienceGained -= UpdateLevel;
            }
        }

        private void UpdateLevel()
        {
            int newLevel = CalculateLevel();
            if (newLevel > curentLevel.value)
            {
                curentLevel.value = newLevel;
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

        public float GetStat(Stat stat) => GetBaseStat(stat) + GetAdditiveModifier(stat) * (1 + GetPercentageModifier(stat) / 100);

        public int GetLevel() => curentLevel.value;
        

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
