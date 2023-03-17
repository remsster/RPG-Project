using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName="Progression",menuName="RPG Project/Progression", order=0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] ProgressionCharacterClass[] characterClasses;

        [System.Serializable]
        class ProgressionCharacterClass
        {
            [SerializeField] private CharacterClass characterClass;
            [SerializeField] private float[] health;
        }
    }

}

