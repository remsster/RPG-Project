using RPG.Control;
using UnityEngine;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float speed = 1f;
        private Transform target;

        private void Start()
        {
            target = FindObjectOfType<PlayerController>().transform;
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
    }
}

