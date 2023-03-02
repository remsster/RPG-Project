using UnityEngine;

namespace RPG.Saving
{
    [ExecuteAlways]
    public class SaveableEntity : MonoBehaviour
    {
        [SerializeField] string uniqueIdentifier = "";

        public string GetUniqueIdentifier() => "";

        private void Update()
        {
            if (Application.IsPlaying(gameObject)) return;
            print("Editing");
        }

        public object CaptureState()
        {
            print("Capturing state for " + GetUniqueIdentifier());
            return null;
        }

        public void RestoreState(object state)
        {
            print("Restorign state for " + GetUniqueIdentifier());
        }
    }
}
