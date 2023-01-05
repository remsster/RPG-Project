using UnityEngine;
using UnityEngine.AI;

public class Mover : MonoBehaviour
{
    [SerializeField] private Transform target;

    private NavMeshAgent navMeshAgent;
    private Ray lastRay;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lastRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        }
        Debug.DrawRay(lastRay.origin, lastRay.direction * 100);
        MoveToTarget();
    }

    private void MoveToTarget()
    {
        navMeshAgent.destination = target.position;
    }
}
