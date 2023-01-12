using UnityEngine;

namespace RPG.Core
{
    public class ActionScheduler : MonoBehaviour
    {
        MonoBehaviour currentAction = null;

        public void StartAction(MonoBehaviour action)
        {
            // Don't cancel the same action
            // e.g Mover cancelling Mover
            if (currentAction == action) { return; }

            if (currentAction != null)
            {
                // Cancel the current action
                print("Canceling" + currentAction);
            }
            // Set the new action to the current action
            currentAction = action;
        }
    }
}

