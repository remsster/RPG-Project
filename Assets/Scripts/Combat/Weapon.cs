using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon",order = 0)]
    public class Weapon : ScriptableObject
    {
        // things that will change based on the weapon that we are weilding
        [SerializeField] private AnimatorOverrideController animatorOverride = null;
        [SerializeField] private GameObject equippedPrefab = null;
        [SerializeField] private float weaponRange = 2f;
        [SerializeField] private float weaponDamage = 5f;
        [SerializeField] private bool isRightHanded = true;

        public float Range => weaponRange;
        public float Damage => weaponDamage;

        public void Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            if (equippedPrefab != null)
            {
                Transform handTransform = isRightHanded ? rightHand : leftHand;
                Instantiate(equippedPrefab, handTransform);
            }
            if (animatorOverride != null)
            animator.runtimeAnimatorController = animatorOverride;
        }
    }
}
