using UnityEngine;

namespace RPG.Core
{
    public class ActionScheduler : MonoBehaviour
    {
        IAction currentAction = null;

        public void StartAction(IAction action)
        {
            // Don't cancel the same action
            // e.g Mover cancelling Mover
            if (currentAction == action) { return; }

            if (currentAction != null)
            {
                // Cancel the current action
                currentAction.Cancel();
            }
            // Set the new action to the current action
            currentAction = action;
        }
    }
}

