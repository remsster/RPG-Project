using System;
using UnityEngine;

using GameDevTV.Utils;

using RPG.Movement;
using RPG.Combat;
using RPG.Core;
using RPG.Attributes;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] private float chaseDistance = 5f;
        [SerializeField] private float suspicionTime = 5f;
        [SerializeField] private float waypointTollerance = 1f; // its basically the stopping distance
        [SerializeField] private float dwellTime = 5f;
        [SerializeField] private float aggrevatedCooldownTime = 5f;
        [SerializeField] private float shoutDistance = 5f;
        [Range(0, 1)]
        [SerializeField] private float patrolSpeedFraction = 0.2f;
        [SerializeField] private PatrolPath patrolPath;

        private GameObject player;
        private Fighter fighter;
        private Health health;
        private Mover mover;
        private LazyValue<Vector3> guardPosition;
        private int currentWaypointIndex = 0;

        private float timeSinceLastSawPlayer = Mathf.Infinity;
        private float timeSinceAggrevated = Mathf.Infinity;
        private float timeSpentAtWaypoint = Mathf.Infinity;

        // ---------------------------------------------------------------------------------
        // Unity Engine Methods
        // ---------------------------------------------------------------------------------

        private void Awake()
        {
            player = GameObject.FindWithTag("Player");
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();
            guardPosition = new LazyValue<Vector3>(GetGuardPosition);
        }

        private void Start()
        {
            guardPosition.ForceInit();
        }

        private void Update()
        {
            if (health.IsDead()) { return; }
            if (IsAggrevated() && fighter.CanAttack(player))
            {
                AttackBehaviour();
            }
            else if (timeSinceLastSawPlayer < suspicionTime)
            {
                SuspcicionBehaviour();
            }
            else
            {
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

        private Vector3 GetGuardPosition()
        {
            return transform.position;
        }

        private void UpdateTimers()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSpentAtWaypoint += Time.deltaTime;
            timeSinceAggrevated += Time.deltaTime;
        }

        private bool IsAggrevated()
        {
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            return distanceToPlayer < chaseDistance || timeSinceAggrevated < aggrevatedCooldownTime;
        }

        private void PatrolBehaviour()
        {
            Vector3 nextPosition = guardPosition.value;

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

            AggrevateNearbyEnemies();
        }

        private void AggrevateNearbyEnemies()
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, shoutDistance, Vector3.up, 0);
            foreach(RaycastHit hit in hits)
            {
                if (hit.transform.TryGetComponent<AIController>(out AIController ai))
                {
                    ai.Aggrevate();
                }
            }
        }

        // ---- Public ----

        public void Aggrevate()
        {
            timeSinceAggrevated = 0;
        }
    }

}
