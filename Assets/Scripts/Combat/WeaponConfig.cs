using UnityEngine;

using RPG.Attributes;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon",order = 0)]
    public class WeaponConfig : ScriptableObject
    {
        // things that will change based on the weapon that we are weilding
        [SerializeField] private AnimatorOverrideController animatorOverride;
        [SerializeField] private Weapon equippedPrefab;
        [SerializeField] private float weaponRange = 2f;
        [SerializeField] private float weaponDamage = 5f;
        [SerializeField] private float percentageBonus = 0;
        [SerializeField] private bool isRightHanded = true;
        [SerializeField] Projectile projectile;

        private const string WEAPON_NAME = "Weapon";

        private Transform GetTransform(Transform rightHand, Transform leftHand) => isRightHanded ? rightHand : leftHand;

        public float Range => weaponRange;
        public float Damage => weaponDamage;
        public float PercentageBonus => percentageBonus;
        public bool HasProjectile => projectile != null;

        private void DestroyOldWeapon(Transform rightHand, Transform leftHand)
        {
            Transform oldWeapon = rightHand.Find(WEAPON_NAME);
            if (oldWeapon == null)
            {
                oldWeapon = leftHand.Find(WEAPON_NAME);
            }
            if (oldWeapon == null) return;
            oldWeapon.name = "DESTROYING";
            Destroy(oldWeapon.gameObject);
        }

        public void Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            DestroyOldWeapon(rightHand, leftHand);
            if (equippedPrefab != null)
            {
                Transform handTransform = GetTransform(rightHand, leftHand);
                Weapon weapon = Instantiate(equippedPrefab, handTransform);
                weapon.gameObject.name = WEAPON_NAME;
            }

            // this will be null if it's the root animator controller
            // otherwise it will have the AnimatorOverrideController
            AnimatorOverrideController overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;

            if (animatorOverride != null)
            {
                animator.runtimeAnimatorController = animatorOverride;
            }
            else if (overrideController != null)
            {
                // go back to the root class and put it in the animation slot
                // (basically go back to the default unarmed animation)
                animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
            }
        }

        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target, GameObject instigator, float calculatedDamage)
        {
            Projectile projectileInstance = Instantiate(projectile, GetTransform(rightHand, leftHand).position,Quaternion.identity);
            projectileInstance.SetTarget(target, instigator, calculatedDamage);
        }
    }
}
