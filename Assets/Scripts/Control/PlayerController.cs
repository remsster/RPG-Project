using UnityEngine;
using UnityEngine.EventSystems;

using RPG.Movement;
using RPG.Attributes;
using System;
using UnityEngine.AI;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        private Health health;

        

        [System.Serializable]
        private struct CursorMapping
        {
            public CursorType type;
            public Texture2D texture;
            public Vector2 hotspot;
        }

        [SerializeField] private CursorMapping[] cursorMappings;
        [SerializeField] private float maxNavMeshProjectionDistance = 1f;
        [SerializeField] private float raycastRadius = 1f;
        


        // ---------------------------------------------------------------------------------
        // Unity Engine Methods
        // ---------------------------------------------------------------------------------

        private void Awake()
        {
            health = GetComponent<Health>();
        }

        private void Update()
        {
            if (InteractWithComponent()) return;
            if (InteractWithUI()) return;
            if (health.IsDead()) 
            {
                SetCursor(CursorType.None);
                return; 
            }
            if (InteractWithMovement()) return;
            SetCursor(CursorType.None);
        }

        // ---------------------------------------------------------------------------------
        // Custom Methods
        // ---------------------------------------------------------------------------------

        // ---- Private ----

        private bool InteractWithUI()
        {
            // Is this over a UI GameObject
            if (EventSystem.current.IsPointerOverGameObject())
            {
                SetCursor(CursorType.UI);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Generic interaction method that interacts with enemies and pickups
        /// possible to scale using IRaycastable
        /// </summary>
        /// <returns>Returns true if the GameIbject is interactable and false otherwise</returns>
        private bool InteractWithComponent()
        {
            RaycastHit[] hits = RaycastAllSorted();
            foreach (RaycastHit hit in hits)
            {
                IRaycastable[] raycastables = hit.transform.GetComponents<IRaycastable>();
                foreach (IRaycastable raycastable in raycastables)
                {
                    if (raycastable.HandleRayCast(this))
                    {
                        SetCursor(raycastable.GetCursorType());
                        return true;
                    }
                }
            }
            return false;
        }

        private bool InteractWithMovement()
        {
            Vector3 target;
            bool hasHit = RaycastNavMesh(out target);
            if (hasHit)
            {
                if (!GetComponent<Mover>().CanMoveTo(target)) return false;

                if (Input.GetMouseButton(0))
                {
                    GetComponent<Mover>().StartMoveAction(target,1f);
                }
                SetCursor(CursorType.Movement);
                return true;
            }
            return false;
        }

        private bool RaycastNavMesh(out Vector3 target)
        {
            target = new Vector3();
            // Raycast to terrain
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(),out hit);
            if (!hasHit) return false;
            // Find nearist navmesh point
            NavMeshHit navMeshHit;
            bool hasCastToNavMesh = NavMesh.SamplePosition(hit.point, out navMeshHit, maxNavMeshProjectionDistance, NavMesh.AllAreas);
            if (!hasCastToNavMesh) return false;
            target = navMeshHit.position;

            //NavMeshPath path = new NavMeshPath();
            //bool hasPath = NavMesh.CalculatePath(transform.position, target, NavMesh.AllAreas, path);
            //if (!hasPath) return false;
            //if (path.status != NavMeshPathStatus.PathComplete) return false;
            //if (GetPathLength(path) > maxNavMeshPathLength) return false;

            return true;
        }

        

        /// <summary>
        /// Sort raycast GameObjects by distance
        /// </summary>
        /// <returns>Sorted raycast array</returns>
        private RaycastHit[] RaycastAllSorted()
        {
            // Get all hits
            RaycastHit[] hits = Physics.SphereCastAll(GetMouseRay(),raycastRadius);
            // sort by distance
            // -- build the array of distances
            float[] distances = new float[hits.Length];
            for (int i = 0; i < hits.Length; i++)
            {
                distances[i] = hits[i].distance;
            }
            // -- sort the hits
            Array.Sort(distances, hits);
            // return array
            return hits;
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }

        private void SetCursor(CursorType type)
        {
            CursorMapping mapping = GetCursorMapping(type);
            Cursor.SetCursor(mapping.texture, mapping.hotspot, CursorMode.Auto);
        }

        private CursorMapping GetCursorMapping(CursorType type)
        {
            foreach(CursorMapping mapping in cursorMappings)
            {
                if (type == mapping.type)
                {
                    return mapping;
                }
            }
            return cursorMappings[0];
        }
    }
}



