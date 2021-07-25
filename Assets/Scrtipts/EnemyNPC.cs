using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Sub class of NPC, specific for Enemy NPC.
// The class handles the target of the NPC based on patrol points
public class EnemyNPC : NPC
{
    // All the positions, the NPC would walk too in the game (patrol points).
    public Transform[] walkingTargets;

    protected override void AcquireTarget()
    {
        Transform target = null;
        // Reset the stopping distance so the NPC would get as close as possible
        agent.stoppingDistance = 1f;
        if (!hasGun)
        {
            // If the NPC doesn't have a gun, search for the closest gun first (NPC can't fight without a gun)
            List<Transform> gunPositions = gunPlacer.GetGunPositions();
            // Because we search for minimum value, the initial value would be as high as possible (infinite)
            float shortestDistance = 9999;
            Transform closestGun = null;
            for (int i = 0; i < gunPositions.Count; i++)
            {
                // If this gun is closer
                if (Vector3.Distance(gunPositions[i].position, transform.position) < shortestDistance)
                {
                    // Fist check if the NPC can get to that gun
                    NavMeshPath path = new NavMeshPath();
                    agent.CalculatePath(gunPositions[i].position, path);
                    // If the path is completed, it means the character can reach that gun position
                    if (path.status == NavMeshPathStatus.PathComplete)
                    {
                        // Update the variables
                        closestGun = gunPositions[i];
                        shortestDistance = Vector3.Distance(gunPositions[i].position, transform.position);
                    }
                }
            }
            if (closestGun != null)
            {
                // If we found a gun, make it the target and remove it from the list (so no other NPC would try to take it)
                target = closestGun;
                gunPositions.Remove(closestGun);
            }
        }
        else
        {
            // If the NPC has a gun, walk to some random patrol point.
            target = walkingTargets[Random.Range(0, walkingTargets.Length)];
        }
        if (target != null)
        {
            // Instruct the NavMeshAgent to go to the target
            agent.SetDestination(target.position);
        }
    }
}