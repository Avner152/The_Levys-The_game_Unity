using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

// Sub class of NPC, specific for Player NPC (the one that follows the player).
// The class handles the target of the NPC based on the player position
public class PlayerNPC : NPC
{
    public Transform player;

    protected override void AcquireTarget()
    {
        // The default target is the player
        Transform target = player;
        // And we don't want to stop too close to it
        agent.stoppingDistance = 2.5f;
        if (!hasGun)
        {
            // If the NPC doesn't have a gun, search for the closest gun first
            List<Transform> gunPositions = gunPlacer.GetGunPositions();
            float shortestDistance = 9999;
            Transform closestGun = null;
            for (int i = 0; i < gunPositions.Count; i++)
            {
                if (Vector3.Distance(gunPositions[i].position, transform.position) < shortestDistance)
                {
                    // Fist check if th NPC can get to that gun
                    NavMeshPath path = new NavMeshPath();
                    agent.CalculatePath(gunPositions[i].position, path);
                    // If the path is completed, it means the character can reach that gun
                    if (path.status == NavMeshPathStatus.PathComplete)
                    {
                        closestGun = gunPositions[i];
                        shortestDistance = Vector3.Distance(gunPositions[i].position, transform.position);
                    }
                }
            }
            if (closestGun != null)
            {
                target = closestGun;
            }
        }
        if (target != null)
        {
            // Check if th NPC can get to target
            NavMeshPath path = new NavMeshPath();
            agent.CalculatePath(target.position, path);
            // If the path is completed, it means the character can reach the target
            if (path.status == NavMeshPathStatus.PathComplete)
            {
                agent.SetDestination(target.position);
            }
        }
    }
}
