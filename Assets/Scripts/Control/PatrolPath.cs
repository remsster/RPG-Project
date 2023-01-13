using UnityEngine;

namespace RPG.Control
{
    public class PatrolPath : MonoBehaviour
    {
        private const float RADIUS = 0.3F;

        private void OnDrawGizmos()
        {
            for(int i = 0; i < transform.childCount; i++)
            {
                int j = GetNextIndex(i, transform.childCount);
                Gizmos.DrawSphere(GetWaypoint(i), RADIUS);
                Gizmos.DrawLine(GetWaypoint(i), GetWaypoint(j));
            }
        }

        private int GetNextIndex(int i, int maxWaypoints)
        {
            int nextIndex = i + 1;
            if (nextIndex == maxWaypoints) nextIndex = 0;
            return nextIndex;
        }

        private Vector3 GetWaypoint(int i)
        {
            return transform.GetChild(i).position;
        }
    }
}

