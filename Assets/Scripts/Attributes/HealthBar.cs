using UnityEngine;

namespace RPG.Attributes
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private RectTransform foregroundImage;
        [SerializeField] private Health healthComponent;

        private void Update()
        {
            foregroundImage.localScale = new Vector3(healthComponent.GetFraction(),1f,1f);
        }
    }
}
