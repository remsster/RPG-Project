using UnityEngine;

namespace RPG.Attributes
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private RectTransform foregroundImage;
        [SerializeField] private Health healthComponent;
        [SerializeField] private Canvas rootCanvas;

        private void Update()
        {
            if (Mathf.Approximately(healthComponent.GetFraction(),0) || Mathf.Approximately(healthComponent.GetFraction(), 1))
            {
                rootCanvas.enabled = false;
                return;
            }
            rootCanvas.enabled = true;
            foregroundImage.localScale = new Vector3(healthComponent.GetFraction(),1f,1f);

        }
    }
}
