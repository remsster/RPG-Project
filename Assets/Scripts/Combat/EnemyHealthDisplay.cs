using RPG.Attributes;
using TMPro;
using UnityEngine;

namespace RPG.Combat
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        private Fighter fighter;
        
        private TextMeshProUGUI healthValue;

        private void Awake()
        {
            fighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
            healthValue = GetComponent<TextMeshProUGUI>();
        }

        private void Update()
        {
            if (fighter.Target == null)
            {
                healthValue.text = "N/A";
                return;
            }
            Health health = fighter.Target;
            healthValue.text = $"{health.HealthPoints.ToString("0")}/{health.MaxHealthPoints.ToString("0")}";
        }
    }
}
