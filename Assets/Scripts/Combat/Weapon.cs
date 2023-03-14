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

        private const string WEAPON_NAME = "Weapon";

        private Transform GetTransform(Transform rightHand, Transform leftHand) => isRightHanded ? rightHand : leftHand;

        public float Range => weaponRange;
        public float Damage => weaponDamage;
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
                GameObject weapon = Instantiate(equippedPrefab, handTransform);
                weapon.name = WEAPON_NAME;
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
