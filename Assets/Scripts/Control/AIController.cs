using UnityEngine;


using RPG.Movement;
using RPG.Combat;
using RPG.Core;
using System;
using UnityEngine.AI;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] private float chaseDistance = 5f;
        [SerializeField] private float suspicionTime = 5f;
        [SerializeField] private float waypointTollerance = 1f;
        [SerializeField] private float dwellTime = 5f;
        [Range(0, 1)]
        [SerializeField] private float patrolSpeedFraction = 0.2f;
        [SerializeField] private PatrolPath patrolPath;

        private GameObject player;
        private Fighter fighter;
        private Health health;
        private Mover mover;
        private NavMeshAgent navMeshAgent;
        private Vector3 guardPosition;
        private int currentWaypointIndex = 0;

        private float timeSinceLastSawPlayer = Mathf.Infinity;
        private float timeSpentAtWaypoint = Mathf.Infinity;

        // ---------------------------------------------------------------------------------
        // Unity Methods
        // ---------------------------------------------------------------------------------

        private void Awake()
        {
            player = GameObject.FindWithTag("Player");
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();
            navMeshAgent = GetComponent<NavMeshAgent>();
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
                //navMeshAgent.speed = chaseSpeed;
                AttackBehaviour();
            }
            else if (timeSinceLastSawPlayer < suspicionTime)
            {
                SuspcicionBehaviour();
            }
            else
            {
                // navMeshAgent.speed = patrolSpeed;
                PatrolBehaviour();
            }

            UpdateTimers();
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

        private void UpdateTimers()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSpentAtWaypoint += Time.deltaTime;
        }

        private bool InAttackRange()
        {
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            return distanceToPlayer < chaseDistance;
        }

        private void PatrolBehaviour()
        {
            Vector3 nextPosition = guardPosition;

            if (patrolPath != null)
            {
                if (AtWaypoint())
                {                    
                    CycleWaypoint();
                    timeSpentAtWaypoint = 0;
                }
                nextPosition = GetCurrentWaypoint();
            }
            if (timeSpentAtWaypoint > dwellTime)
            {
                mover.StartMoveAction(nextPosition, patrolSpeedFraction);
            }
        }

        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypoint(currentWaypointIndex);
        }

        private void CycleWaypoint()
        {
            currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
        }

        private bool AtWaypoint()
        {
            return Vector3.Distance(transform.position, GetCurrentWaypoint()) < waypointTollerance;
        }

        private void SuspcicionBehaviour()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AttackBehaviour()
        {
            timeSinceLastSawPlayer = 0;
            fighter.Attack(player);
        }
    }

}
