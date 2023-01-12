

using RPG.Movement;
using System;
using UnityEngine;

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
            
            if (target != null && !IsInRange(target))
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

        public void Attack(CombatTarget combatTarget)
        {
            target = combatTarget.transform;
            if (target != null)
            {

            }
        }

        public bool IsInRange(Transform target)
        {
            //float distance = Vector3.Distance(transform.position, target.position);
            float distance = (transform.position - target.position).magnitude;
            return (distance < weaponRange);
        }
    }
}