using UnityEngine;

namespace RPG.UI.DamageText
{
    public class DamageTextSpawner : MonoBehaviour
    {
        [SerializeField] DamageText damageTextPrefab;

        private void Start()
        {
            Spawn(345);
        }

        
        public void Spawn(float damageAmoune)
        {
            DamageText instance = Instantiate<DamageText>(damageTextPrefab, transform);
            
        }
    }
}
