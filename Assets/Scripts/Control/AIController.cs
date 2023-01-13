using UnityEngine;


using RPG.Movement;
using RPG.Combat;
using RPG.Core;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] private float chaseDistance = 5f;

        private GameObject player;
        private Fighter fighter;
        private Health health;
        private Mover mover;
        private Vector3 guardPosition;

        private bool shouldChase = false;

        // ---------------------------------------------------------------------------------
        // Unity Methods
        // ---------------------------------------------------------------------------------

        private void Awake()
        {
            player = GameObject.FindWithTag("Player");
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();
        }

        private void Start()
        {
            guardPosition = transform.position;
        }

        private void Update()
        {
            if (health.IsDead) { return; }
            if (InAttackRange() && fighter.CanAttack(player))
            {
                fighter.Attack(player);
            }
            else
            {
                mover.StartMoveAction(guardPosition);
            }
        }

        
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }


        // ---------------------------------------------------------------------------------
        // Custom Methods
        // ---------------------------------------------------------------------------------

        // ---- Private ----

        private bool InAttackRange()
        {
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            return distanceToPlayer < chaseDistance;
        }
    }

}
