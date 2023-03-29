using UnityEngine;

namespace RPG.Control
{
    public class PatrolPath : MonoBehaviour
    {
        private const float RADIUS = 0.3F;

        // ---------------------------------------------------------------------------------
        // Unity Engine Methods
        // ---------------------------------------------------------------------------------

        private void OnDrawGizmos()
        {
            for(int i = 0; i < transform.childCount; i++)
            {
                int j = GetNextIndex(i);
                Gizmos.DrawSphere(GetWaypoint(i), RADIUS);
                Gizmos.DrawLine(GetWaypoint(i), GetWaypoint(j));
            }
        }

        // ---------------------------------------------------------------------------------
        // Custom Methods
        // ---------------------------------------------------------------------------------

        public int GetNextIndex(int i)
        {
            int nextIndex = i + 1;
            if (nextIndex == transform.childCount) nextIndex = 0;
            return nextIndex;
        }

        public Vector3 GetWaypoint(int i)
        {
            return transform.GetChild(i).position;
        }
    }
}

