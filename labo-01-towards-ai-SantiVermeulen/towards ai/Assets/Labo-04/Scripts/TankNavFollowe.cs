using UnityEngine;
using UnityEngine.AI;

public class TankNavFollower : MonoBehaviour
{
    public Transform playerTarget;
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (playerTarget != null)
        {
            agent.SetDestination(playerTarget.position);
        }
    }
}