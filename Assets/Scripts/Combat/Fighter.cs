using UnityEngine;

using RPG.Movement;
using RPG.Core;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {

        [SerializeField] private float weaponRange = 2f;

        private readonly int attackTriggerHash = Animator.StringToHash("attack");

        private Transform target;

        // ---------------------------------------------------------------------------------
        // Unity Engine Methods
        // ---------------------------------------------------------------------------------

        private void Update()
        {
            if (target == null) return;

            if (!IsInRange(target))
            {
                GetComponent<Mover>().MoveTo(target.position);
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
            GetComponent<Animator>().SetTrigger(attackTriggerHash);
        }

        private bool IsInRange(Transform target)
        {
            //float distance = Vector3.Distance(transform.position, target.position);
            float distance = (transform.position - target.position).magnitude;
            return (distance < weaponRange);
        }

        public void Attack(CombatTarget combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.transform;
        }

        // Animation Event
        private void Hit()
        {
            Debug.Log("Attack!!!");
        }

        // ---- Public ----

        // ---- IAction Methods ----
        public void Cancel()
        {
            target = null;
        }
    }
}