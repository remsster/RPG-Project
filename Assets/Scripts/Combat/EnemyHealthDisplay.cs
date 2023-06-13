using TMPro;
using UnityEngine;

using RPG.Attributes;

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
            healthValue.text = $"{health.GetHealthPoints().ToString("0")}/{health.GetMaxHealthPoints().ToString("0")}";
        }
    }
}
