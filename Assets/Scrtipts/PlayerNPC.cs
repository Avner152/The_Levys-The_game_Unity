using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class PlayerNPC : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public ThirdPersonCharacter character;
    public GunPlacer gunPlacer;
    public Transform gunHolder;
    private bool hasGun = false;

    void Update()
    {
        Transform target = player;
        if (!hasGun)
        {
            // If the character doesn't have a gun, search for the closest gun first
            List<Transform> gunPositions = gunPlacer.GetGunPositions();
            for (int i = 0; i < gunPositions.Count; i++)
            {
                if (Vector3.Distance(gunPositions[i].position, transform.position) <= Vector3.Distance(player.position, transform.position))
                {
                    // Fist check if th NPC can get to that gun.
                    NavMeshPath path = new NavMeshPath();
                    agent.CalculatePath(gunPositions[i].position, path);
                    if (path.status == NavMeshPathStatus.PathComplete)
                    {
                        target = gunPositions[i];
                        break;
                    }
                }
            }
        }
        if (Vector3.Distance(target.position, transform.position) > agent.stoppingDistance)
        {
            agent.SetDestination(target.position);
        }
        if (agent.remainingDistance > agent.stoppingDistance)
        {
            // Tell the character to move
            character.Move(agent.desiredVelocity);
        }
        else
        {
            character.Move(Vector3.zero);
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
        }
    }
}
