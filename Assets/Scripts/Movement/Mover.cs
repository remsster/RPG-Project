using UnityEngine;
using UnityEngine.AI;

using RPG.Core;
using RPG.Saving;
using System.Collections.Generic;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction, ISaveable
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
        // Interface Methods
        // ---------------------------------------------------------------------------------

        // ---- IAction ----
        public void Cancel()
        {
            navMeshAgent.isStopped = true;
        }

        [System.Serializable]
        struct MoverSaveData
        {
            public SerializableVector3 position;
            public SerializableVector3 rotation;
        }

        // ---- ISaveable ----
        public object CaptureState()
        {
            // Saving with Dictionary
            /* 
            Dictionary<string, object> data = new Dictionary<string, object>();
            data["position"] = new SerializableVector3(transform.position);
            data["rotation"] = new SerializableVector3(transform.eulerAngles);
            */

            // Saving with struct
            MoverSaveData data = new MoverSaveData();
            data.position = new SerializableVector3(transform.position);
            data.rotation = new SerializableVector3(transform.eulerAngles);

            //return data;
            return new SerializableVector3(transform.position);
        }

        public void RestoreState(object state)
        {
            // Loading with Dictionary
            /*
            Dictionary<string, object> data = (Dictionary<string,object>)state;
            SerializableVector3 position = (SerializableVector3)data["position"];
            SerializableVector3 rotation = (SerializableVector3)data["rotation"];
            */

            // Loading with Struct
            /*
            MoverSaveData data = (MoverSaveData)state;
            SerializableVector3 position = data.position;
            SerializableVector3 rotation = data.rotation;
            */

            SerializableVector3 position = (SerializableVector3)state;
            GetComponent<NavMeshAgent>().enabled = false;
            transform.position = position.ToVector();
            //transform.eulerAngles = rotation.ToVector();
            GetComponent<NavMeshAgent>().enabled = true;

            
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

