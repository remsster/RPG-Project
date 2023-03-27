using TMPro;
using UnityEngine;

namespace RPG.Stats
{
    public class ExperienceDisplay : MonoBehaviour
    {
        private Experience experience;
        private TextMeshProUGUI experienceValue;

        private void Awake()
        {
            experience = GameObject.FindWithTag("Player").GetComponent<Experience>();
            experienceValue = GetComponent<TextMeshProUGUI>();
        }

        private void Update()
        {
            experienceValue.text = $"{experience.ExperiencePoints}";
        }
    }
}
