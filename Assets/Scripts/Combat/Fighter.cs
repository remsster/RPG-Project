using UnityEngine;

using RPG.Movement;
using RPG.Core;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour
    {

        [SerializeField] private float weaponRange = 2f;

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
                GetComponent<Mover>().Stop();
            }
        }

        // ---------------------------------------------------------------------------------
        // Custom Methods
        // ---------------------------------------------------------------------------------

        // ---- Private ----

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

        // ---- Public ----

        public void Cancel()
        {
            target = null;
        }
    }
}