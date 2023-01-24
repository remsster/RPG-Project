using System.Collections.Generic;
using UnityEngine;

using RPG.Saving;

namespace RPG.Core
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] private float healthPoints = 100f;

        private readonly int deathTriggerHash = Animator.StringToHash("die");
        
        private bool isDead;

        public bool IsDead => isDead;

        // ---------------------------------------------------------------------------------
        // Interface Methods
        // ---------------------------------------------------------------------------------

        // ---- ISaveable ----
        public object CaptureState()
        {
            return healthPoints;
        }

        public void RestoreState(object state)
        {
            healthPoints = (float)state;
            if (healthPoints == 0 && !isDead)
            {
                Die();
            }

        }

        // ---------------------------------------------------------------------------------
        // Custom Methods
        // ---------------------------------------------------------------------------------

        // ---- Private ----

        private void Die()
        {
            isDead = true;
            GetComponent<Animator>().SetTrigger(deathTriggerHash);
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        // ---- Public ----

        public void TakeDamage(float damage)
        {
            healthPoints = Mathf.Max(healthPoints - damage, 0);
            if (healthPoints == 0 && !isDead)
            {
                Die();
            }
        }
    }
}
