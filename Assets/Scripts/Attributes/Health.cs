﻿using UnityEngine;
using UnityEngine.Events;

using GameDevTV.Utils;

using RPG.Saving;
using RPG.Stats;
using RPG.Core;
using System;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] private float regenerationPercentage = 70f;
        [SerializeField] private TakeDamageEvent takeDamage;
        [SerializeField] private UnityEvent onDie;

        [System.Serializable]
        public class TakeDamageEvent : UnityEvent<float> { }

        private LazyValue<float> healthPoints;
        private bool isDead;
        private BaseStats baseStats;
        private readonly int deathTriggerHash = Animator.StringToHash("die");

        // ---------------------------------------------------------------------------------
        // Unity Engine Methods
        // ---------------------------------------------------------------------------------

        private void Awake()
        {
            baseStats = GetComponent<BaseStats>();
            healthPoints = new LazyValue<float>(GetInitialHealth);
        }

        private void Start()
        {
            healthPoints.ForceInit();
        }

        private void OnEnable()
        {
            baseStats.onLevelUp += RegenerateHealth;
        }

        private void OnDisable()
        {
            baseStats.onLevelUp += RegenerateHealth;
        }

        // ---------------------------------------------------------------------------------
        // Custom Methods
        // ---------------------------------------------------------------------------------

        // ---- Private ----

        private float GetInitialHealth()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        private void RegenerateHealth()
        {
            float regenHealthPoints = baseStats.GetStat(Stat.Health) * (regenerationPercentage / 100);
            healthPoints.value = Mathf.Max(healthPoints.value, regenHealthPoints);
        }

        private void Die()
        {
            isDead = true;
            GetComponent<Animator>().SetTrigger(deathTriggerHash);
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AwardExperience(GameObject instigator)
        {
            Experience experience = instigator.GetComponent<Experience>();
            if (experience == null) return;
            experience.GainExperience(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
        }

        // ---- Public ----

        public bool IsDead() => isDead;

        public float GetHealthPoints() => healthPoints.value;

        public float GetMaxHealthPoints() => GetComponent<BaseStats>().GetStat(Stat.Health);

        public float GetHealthPercentage() => 100 * GetFraction();

        public float GetFraction() => (healthPoints.value / GetComponent<BaseStats>().GetStat(Stat.Health));

        public void Heal(float healthToRestore)
        {
            healthPoints.value = Mathf.Min(healthPoints.value + healthToRestore, GetMaxHealthPoints());
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            healthPoints.value = Mathf.Max(healthPoints.value - damage, 0);
            if (healthPoints.value == 0)
            {
                Die();
                onDie.Invoke();
                AwardExperience(instigator);       
            }
            else
            {
                takeDamage.Invoke(damage);
            }
        }

        // ---- ISaveable ----

        public object CaptureState()
        {
            return healthPoints.value;
        }

        public void RestoreState(object state)
        {
            healthPoints.value = (float)state;
            if (healthPoints.value <= 0)
            {
                Die();
            }
        }

        
    }
}
