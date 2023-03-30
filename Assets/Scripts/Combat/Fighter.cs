using UnityEngine;

using GameDevTV.Utils;

using RPG.Movement;
using RPG.Core;
using RPG.Saving;
using RPG.Attributes;
using RPG.Stats;
using System.Collections.Generic;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction , ISaveable, IModifierProvider
    {

        [SerializeField] private float timeBetweenAttacks = 1f;
        [SerializeField] private Transform rightHandTransform;
        [SerializeField] private Transform leftHandTransform;
        [SerializeField] private string defaultWeaponName = "Unarmed";
        // default weapon is assigned in the engine
        // the default is unarmed
        [SerializeField] private Weapon defaultWeapon;

        private readonly int attackTriggerHash = Animator.StringToHash("attack");
        private readonly int stopAttackHash = Animator.StringToHash("stopAttack");

        private Health target;
        private float timeSinceLastAttack = Mathf.Infinity;
        private LazyValue<Weapon> currentWeapon;

        // ---------------------------------------------------------------------------------
        // Unity Engine Methods
        // ---------------------------------------------------------------------------------

        private void Awake()
        {
            currentWeapon = new LazyValue<Weapon>(SetupDefaultWeapon);
        }

        private void Start()
        {
            currentWeapon.ForceInit();
        }

        private void Update()
        {

            timeSinceLastAttack += Time.deltaTime;

            if (target == null) return;
            if (target.IsDead) { return; }

            if (!IsInRange(target.transform))
            {
                GetComponent<Mover>().MoveTo(target.transform.position,1f);
            }
            else
            {
                GetComponent<Mover>().Cancel();
                AttackBehaviour();
            }
        }


        // ---------------------------------------------------------------------------------
        // Custom Methods
        // ---------------------------------------------------------------------------------

        // ---- Private ----

        private Weapon SetupDefaultWeapon()
        {
            AttachWeapon(defaultWeapon);
            return defaultWeapon;
        }

        private void AttackBehaviour()
        {
            transform.LookAt(target.transform);
            if (timeSinceLastAttack > timeBetweenAttacks)
            {
                // This will trigger the Hit() event
                TriggerAttack();
                timeSinceLastAttack = 0;

            }
        }

        private void TriggerAttack()
        {
            GetComponent<Animator>().ResetTrigger(stopAttackHash);
            GetComponent<Animator>().SetTrigger(attackTriggerHash);
        }

        // Animation Event
        private void Hit()
        {
            if (target == null) { return; }
            float damage = GetComponent<BaseStats>().GetStat(Stat.Damage);
            // float damage = 5f;
            if (currentWeapon.value.HasProjectile)
            {
                currentWeapon.value.LaunchProjectile(rightHandTransform, leftHandTransform, target, gameObject, damage);
            } 
            else
            {

                target.TakeDamage(gameObject, damage);
            }
        }

        // Animation Event
        private void Shoot()
        {
            Hit();
        }

        private bool IsInRange(Transform target)
        {
            //float distance = Vector3.Distance(transform.position, target.position);
            float distance = (transform.position - target.position).magnitude;
            return (distance < currentWeapon.value.Range);
        }

        private void StopAttack()
        {
            GetComponent<Animator>().ResetTrigger(stopAttackHash);
            GetComponent<Animator>().SetTrigger(stopAttackHash);
        }

        // ---- Public ----

        public Health Target => target;
        

        public void Attack(GameObject combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }

        public void EquipWeapon(Weapon weapon)
        {
            currentWeapon.value = weapon;
            AttachWeapon(weapon);
        }

        private void AttachWeapon(Weapon weapon)
        {
            Animator animator = GetComponent<Animator>();
            weapon.Spawn(rightHandTransform, leftHandTransform, animator);
        }

        public bool CanAttack(GameObject combatTarget) 
        {
            if (combatTarget == null) return false;
            Health targetToTest = combatTarget.GetComponent<Health>();
            return combatTarget != null && !targetToTest.IsDead;
        } 

        // ---- IAction Methods ----
        public void Cancel()
        {
            target = null;
            StopAttack();
            GetComponent<Mover>().Cancel();
        }

        // ---- ISaveable Methods ----
        public object CaptureState()
        {
            return currentWeapon.value.name;
        }

        public void RestoreState(object state)
        {
            string weaponName = (string)state;
            Weapon weapon = Resources.Load<Weapon>(weaponName);
            EquipWeapon(weapon);
        }

        // ---- IModifierProvider Methods ----
        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return currentWeapon.value.Damage;
            }
        }

        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return currentWeapon.value.PercentageBonus;
            }
        }
    }
}