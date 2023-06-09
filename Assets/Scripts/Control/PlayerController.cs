using UnityEngine;
using UnityEngine.EventSystems;

using RPG.Movement;
using RPG.Attributes;
using System;

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
            if (health.IsDead) 
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
            Ray ray = GetMouseRay();
            RaycastHit hit;
            bool hasHit = Physics.Raycast(ray, out hit);
            if (hasHit)
            {

                if (Input.GetMouseButton(0))
                {
                    GetComponent<Mover>().StartMoveAction(hit.point,1f);
                }
                SetCursor(CursorType.Movement);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Sort raycast GameObjects by distance
        /// </summary>
        /// <returns>Sorted raycast array</returns>
        private RaycastHit[] RaycastAllSorted()
        {
            // Get all hits
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
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



