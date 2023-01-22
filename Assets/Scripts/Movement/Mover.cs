using UnityEngine;
using UnityEngine.AI;

using RPG.Core;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction
    {
     
        [SerializeField] private float maxSpeed = 5.66f;
        private NavMeshAgent navMeshAgent;
        private Health health;


        private readonly int forwardSpeedHash = Animator.StringToHash("forwardSpeed");

        // ---------------------------------------------------------------------------------
        // Unity Methods
        // ---------------------------------------------------------------------------------

        private void Awake()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            health = GetComponent<Health>();
        }

        void Update()
        {
            navMeshAgent.enabled = !health.IsDead;
            UpdateAnimator();
        }

        // ---------------------------------------------------------------------------------
        // Custom Methods
        // ---------------------------------------------------------------------------------

        // ---- Private ----

        private void UpdateAnimator()
        {
            Vector3 velocity = navMeshAgent.velocity;
            // taking from global and convert it to local
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;
            GetComponent<Animator>().SetFloat(forwardSpeedHash, speed);
        }

        // ---- Public ----


        // ---- IAction ----
        public void Cancel()
        {
            navMeshAgent.isStopped = true;
        }
        

        public void StartMoveAction(Vector3 destination, float speedFraction)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination,speedFraction);
        }


        public void MoveTo(Vector3 destination, float speedFraction)
        {
            navMeshAgent.destination = destination;
            navMeshAgent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
            navMeshAgent.isStopped = false;
        }
    }
}

