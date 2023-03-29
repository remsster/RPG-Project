using UnityEngine;


namespace RPG.Core
{ 
    /// <summary>
    /// Used Before cinemachine
    /// Currently not being used anywhere
    /// Can be deleted after project is finished
    /// </summary>
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField] private Transform target;

        private void LateUpdate()
        {
            transform.position = target.position;
        }
    }
}


