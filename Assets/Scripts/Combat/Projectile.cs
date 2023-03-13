using UnityEngine;
using RPG.Core;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float speed = 1f;
        private Health target;

        private void Start()
        {
            
        }

        private void Update()
        {
            if (target == null) return;
            transform.LookAt(AimLocation);
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        //private Vector3 GetAimLocation()
        //{
        //    CapsuleCollider targetCollider = target.GetComponent<CapsuleCollider>();
        //    if (targetCollider == null) { return target.position; }
        //    return target.position + Vector3.up * targetCollider.height / 2;
        //}

        private Vector3 AimLocation => target.TryGetComponent<CapsuleCollider>(out CapsuleCollider targetCollider) ?
            targetCollider.transform.position + Vector3.up * targetCollider.height / 2 : targetCollider.transform.position;

        public void SetTarget(Health target)
        {
            this.target = target;
        }
    }
}

