using UnityEngine;

using RPG.Core;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon",order = 0)]
    public class Weapon : ScriptableObject
    {
        // things that will change based on the weapon that we are weilding
        [SerializeField] private AnimatorOverrideController animatorOverride;
        [SerializeField] private GameObject equippedPrefab;
        [SerializeField] private float weaponRange = 2f;
        [SerializeField] private float weaponDamage = 5f;
        [SerializeField] private bool isRightHanded = true;
        [SerializeField] Projectile projectile;

        private Transform GetTransform(Transform rightHand, Transform leftHand) => isRightHanded ? rightHand : leftHand;

        public float Range => weaponRange;
        public float Damage => weaponDamage;
        public bool HasProjectile => projectile != null;

        public void Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            if (equippedPrefab != null)
            {
                Transform handTransform = GetTransform(rightHand, leftHand);
                Instantiate(equippedPrefab, handTransform);
            }
            if (animatorOverride != null)
            animator.runtimeAnimatorController = animatorOverride;
        }

        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target)
        {
            Projectile projectileInstance = Instantiate(projectile, GetTransform(rightHand, leftHand).position,Quaternion.identity);
            projectileInstance.SetTarget(target, weaponDamage);
        }
    }
}
