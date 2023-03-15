using UnityEngine;

using RPG.Movement;
using RPG.Core;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {

        [SerializeField] private float timeBetweenAttacks = 1f;
        [SerializeField] private Transform rightHandTransform = null;
        [SerializeField] private Transform leftHandTransform = null;
        [SerializeField] private Weapon defaultWeapon = null;
        [SerializeField] private string defaultWeaponName = "Unarmed";
        

        private readonly int attackTriggerHash = Animator.StringToHash("attack");
        private readonly int stopAttackHash = Animator.StringToHash("stopAttack");

        private Health target;
        private float timeSinceLastAttack = Mathf.Infinity;
        private Weapon currentWeapon;

        // ---------------------------------------------------------------------------------
        // Unity Engine Methods
        // ---------------------------------------------------------------------------------

        private void Start()
        {
            Weapon weapon = Resources.Load<Weapon>(defaultWeaponName);
            EquipWeapon(weapon);
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
            if (currentWeapon.HasProjectile)
            {
                currentWeapon.LaunchProjectile(rightHandTransform, leftHandTransform, target);
            } 
            else
            {

            target.TakeDamage(currentWeapon.Damage);
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
            return (distance < currentWeapon.Range);
        }

        private void StopAttack()
        {
            GetComponent<Animator>().ResetTrigger(stopAttackHash);
            GetComponent<Animator>().SetTrigger(stopAttackHash);
        }

        // ---- Public ----

        public void Attack(GameObject combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }

        public void EquipWeapon(Weapon weapon)
        {
            currentWeapon = weapon;
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

    }
}