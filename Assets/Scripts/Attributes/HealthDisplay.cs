using TMPro;
using UnityEngine;

namespace RPG.Attributes
{
    public class HealthDisplay : MonoBehaviour
    {
        private Health health;
        private TextMeshProUGUI healthValue;

        private void Awake()
        {
            health = GameObject.FindWithTag("Player").GetComponent<Health>();
            healthValue = GetComponent<TextMeshProUGUI>();
        }

        private void Update()
        {
            healthValue.text = $"{health.HealthPoints.ToString("0")}/{health.MaxHealthPoints.ToString("0")}";
        }
    }
}
