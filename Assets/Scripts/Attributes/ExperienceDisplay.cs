using TMPro;
using UnityEngine;

namespace RPG.Attributes
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
