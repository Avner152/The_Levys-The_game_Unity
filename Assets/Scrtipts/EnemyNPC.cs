using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNPC : NPC
{
    public Transform[] walkingTargets;

    protected override void AcquireTarget()
    {
        Transform target = null;
        agent.stoppingDistance = 1f;
        if (!hasGun)
        {
            // If the character doesn't have a gun, search for the closest gun first
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
                gunPositions.Remove(closestGun);
            }
        }
        else
        {
            target = walkingTargets[Random.Range(0, walkingTargets.Length)];
        }
        if (target != null)
        {
            agent.SetDestination(target.position);
        }
    }
}