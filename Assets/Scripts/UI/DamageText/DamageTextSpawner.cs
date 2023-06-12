using UnityEngine;

namespace RPG.UI.DamageText
{
    public class DamageTextSpawner : MonoBehaviour
    {
        [SerializeField] DamageText damageTextPrefab;
        
        public void Spawn(float damageAmoune)
        {
            DamageText instance = Instantiate<DamageText>(damageTextPrefab, transform);
            
        }
    }
}
