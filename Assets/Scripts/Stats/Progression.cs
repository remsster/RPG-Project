using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName="Progression",menuName="RPG Project/Progression", order=0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] ProgressionCharacterClass[] characterClasses;

        public float GetStat(Stat stat, CharacterClass characterClass, int level)
        {
            foreach(ProgressionCharacterClass progressionCharacter in characterClasses)
            {
                if (characterClass != progressionCharacter.CharacterClass) continue;

                foreach(var progressionStat in progressionCharacter.stats)
                {
                    if (progressionStat.stat != stat) continue;
                    // guard againsted array out of bounds
                    if (progressionStat.levels.Length < level) continue;
                    return progressionStat.levels[level - 1];
                }
                
            }
            return 0;
        }

        [System.Serializable]
        class ProgressionCharacterClass
        {
            [SerializeField] private CharacterClass characterClass;
            [SerializeField] public ProgressionStat[] stats;

            public CharacterClass CharacterClass => characterClass;
        }

        [System.Serializable]
        class ProgressionStat
        {
            public Stat stat;
            public float[] levels;
        }
    }

}

