using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class PlayerNPC : NPC
{
    public Transform player;

    protected override void AcquireTarget()
    {
        Transform target = player;
        agent.stoppingDistance = 2.5f;
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
            }
        }
        if (target != null)
        {
            // Fist check if th NPC can get to that gun
            NavMeshPath path = new NavMeshPath();
            agent.CalculatePath(target.position, path);
            // If the path is completed, it means the character can reach that gun
            if (path.status == NavMeshPathStatus.PathComplete)
            {
                agent.SetDestination(target.position);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Gun") && !hasGun)
        {
            hasGun = true;
            other.gameObject.SetActive(false);
            other.transform.parent.SetParent(gunHolder, false);
            other.transform.parent.GetChild(0).localPosition = Vector3.zero;
            other.GetComponentInParent<Rotation>().enabled = false;
            agent.stoppingDistance = 1.75f;
        }
    }
}
