using UnityEngine;
using RPG.Core;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float speed = 1f;
        [SerializeField] private bool isHoaming = false;
        private Health target;
        private float damage = 0;

        // ---------------------------------------------------------------------------------
        // Unity Engine Methods
        // ---------------------------------------------------------------------------------

        private void Start()
        {
            transform.LookAt(AimLocation);
            Destroy(gameObject, 10f);
        }

        private void Update()
        {
            if (target == null) return;
            if (isHoaming && !target.IsDead)
            {
                transform.LookAt(AimLocation);
            }
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        private void OnTriggerEnter(Collider collision)
        {
            if (collision.GetComponent<Health>() != target) return;
            if (target.IsDead) return;
            
            target.TakeDamage(damage);
            Destroy(gameObject);
        }

        // ---------------------------------------------------------------------------------
        // Custom Methods
        // ---------------------------------------------------------------------------------

        //private Vector3 GetAimLocation()
        //{
        //    CapsuleCollider targetCollider = target.GetComponent<CapsuleCollider>();
        //    if (targetCollider == null) { return target.position; }
        //    return target.position + Vector3.up * targetCollider.height / 2;
        //}

        private Vector3 AimLocation => target.TryGetComponent<CapsuleCollider>(out CapsuleCollider targetCollider) ?
            targetCollider.transform.position + Vector3.up * targetCollider.height / 2 : targetCollider.transform.position;

        public void SetTarget(Health target, float damage)
        {
            this.target = target;
            this.damage = damage;
        }
    }
}

