using TMPro;
using UnityEngine;

namespace RPG.Stats
{
    public class LevelDisplay : MonoBehaviour
    {
        private BaseStats baseStats;
        private TextMeshProUGUI levelValue;

        private void Awake()
        {
            baseStats = GameObject.FindWithTag("Player").GetComponent<BaseStats>();
            levelValue = GetComponent<TextMeshProUGUI>();
        }

        private void Update()
        {
            levelValue.text = $"{baseStats.GetLevel()}";
        }
    }
}
