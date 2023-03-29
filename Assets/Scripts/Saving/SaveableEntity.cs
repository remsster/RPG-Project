using System.Collections.Generic;

using UnityEditor;
using UnityEngine;

namespace RPG.Saving
{
    [ExecuteAlways]
    public class SaveableEntity : MonoBehaviour
    {
        [SerializeField] string uniqueIdentifier = "";
        static Dictionary<string, SaveableEntity> globalLookup = new Dictionary<string, SaveableEntity>();

        public string GetUniqueIdentifier() => uniqueIdentifier;

        // ---------------------------------------------------------------------------------
        // Unity Engine Methods
        // ---------------------------------------------------------------------------------

#if UNITY_EDITOR
        // Not included in build
        private void Update()
        {
            if (Application.IsPlaying(gameObject)) return;
            if (string.IsNullOrEmpty(gameObject.scene.path)) return;
            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty property = serializedObject.FindProperty(nameof(uniqueIdentifier));
            
            if (string.IsNullOrEmpty(property.stringValue) || !IsUnique(property.stringValue))
            {
                property.stringValue = System.Guid.NewGuid().ToString();
                serializedObject.ApplyModifiedProperties();
            }

            globalLookup[property.stringValue] = this;

        }
#endif


        // ---------------------------------------------------------------------------------
        // Custom Methods
        // ---------------------------------------------------------------------------------

        private bool IsUnique(string candidate)
        {
            // check if key exist in dictionary
            if (!globalLookup.ContainsKey(candidate)) return true;
            // check if not point to self
            if (globalLookup[candidate] == this) return true;
            if (globalLookup[candidate] == null)
            {
                // has been deleted from the global lookup;
                // by changing scenes
                globalLookup.Remove(candidate);
                return true;
            }
            if (globalLookup[candidate].GetUniqueIdentifier() != candidate)
            {
                globalLookup.Remove(candidate);
                return true;
            }
                return false;
        }

        public object CaptureState()
        {
            Dictionary<string, object> state = new Dictionary<string, object>();
            foreach(ISaveable saveable in GetComponents<ISaveable>())
            {
                state[saveable.GetType().ToString()] = saveable.CaptureState();
            }
            return state;
        }

        public void RestoreState(object state)
        {
            Dictionary<string, object> stateDict = (Dictionary<string, object>)state;
            foreach(ISaveable saveable in GetComponents<ISaveable>())
            {
                string typeString = saveable.GetType().ToString();
                if (stateDict.ContainsKey(typeString))
                {
                    saveable.RestoreState(stateDict[typeString]);
                }
            }
        }
    }
}
