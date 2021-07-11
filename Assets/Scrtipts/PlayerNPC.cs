using UnityEngine;
using UnityEngine.AI;

public class PlayerNPC : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;

    void Update()
    {
        if (Vector3.Distance(player.position, transform.position) > agent.stoppingDistance)
        {
            agent.SetDestination(player.position);
        }
    }
}
