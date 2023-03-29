using UnityEngine;

using RPG.Movement;
using RPG.Combat;
using RPG.Attributes;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        private Health health;

        // ---------------------------------------------------------------------------------
        // Unity Engine Methods
        // ---------------------------------------------------------------------------------

        private void Awake()
        {
            health = GetComponent<Health>();
        }

        private void Update()
        {
            if (health.IsDead) { return; }
            if (InteractWithCombat()) return;
            if (InteractWithMovement()) return;
        }

        // ---------------------------------------------------------------------------------
        // Custom Methods
        // ---------------------------------------------------------------------------------

        // ---- Private ----

        private bool InteractWithCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            foreach (RaycastHit hit in hits)
            {
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();
                if (target == null) continue;

                
                if (!GetComponent<Fighter>().CanAttack(target.gameObject)) continue;

                if (Input.GetMouseButton(0))
                {
                    GetComponent<Fighter>().Attack(target.gameObject);
                }
                return true;
            }
            // We didnt find any combat targets to interact with
            return false;
        }

        private bool InteractWithMovement()
        {
            Ray ray = GetMouseRay();
            RaycastHit hit;
            bool hasHit = Physics.Raycast(ray, out hit);
            if (hasHit)
            {

                if (Input.GetMouseButton(0))
                {
                    GetComponent<Mover>().StartMoveAction(hit.point,1f);
                }
                return true;
            }
            return false;
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}



