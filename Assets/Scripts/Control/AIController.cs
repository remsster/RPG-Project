using UnityEngine;


namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] private float chaseDistance = 5f;

        private bool shouldChase = false;

        // ---------------------------------------------------------------------------------
        // Unity Methods
        // ---------------------------------------------------------------------------------

        private void Update()
        {    
            if (DistanceToPlayer() < chaseDistance)
            {
                print(gameObject.name + " give chase");
            }
        }

        private float DistanceToPlayer()
        {
            GameObject player = GameObject.FindWithTag("Player");
            return Vector3.Distance(player.transform.position, transform.position);
        }
    }

}
