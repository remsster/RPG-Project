using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName="Progression",menuName="RPG Project/Progression", order=0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] ProgressionCharacterClass[] characterClasses;

        public float GetHealth(CharacterClass characterClass, int level)
        {
            foreach(ProgressionCharacterClass progressionCharacter in characterClasses)
            {
                if (characterClass == progressionCharacter.CharacterClass)
                {
                    // return progressionCharacter.Health[level-1];
                }
            }
            return 0;
        }

        [System.Serializable]
        class ProgressionCharacterClass
        {
            [SerializeField] private CharacterClass characterClass;
            [SerializeField] private ProgressionStat[] stats;

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

