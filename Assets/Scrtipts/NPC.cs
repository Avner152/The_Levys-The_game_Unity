using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public abstract class NPC : MonoBehaviour
{
    public NavMeshAgent agent;
    public ThirdPersonCharacter character;
    public GunPlacer gunPlacer;
    public Transform gunHolder;
    public Health health;
    public float attackRange = 3;
    protected bool hasGun;
    private Transform enemy;

    // Start is called before the first frame update
    void Start()
    {
        enemy = null;
        hasGun = false;
        AcquireTarget();
    }

    // Update is called once per frame
    void Update()
    {
        if (health.health > 0)
        {
            if (agent.remainingDistance > agent.stoppingDistance)
            {
                // Tell the character to move
                character.Move(agent.desiredVelocity);
            }
            else
            {
                // Stop the character if it reached the target
                character.Move(Vector3.zero);
                if (enemy == null)
                {
                    AcquireTarget();
                }
                else
                {
                    transform.rotation = Quaternion.LookRotation((enemy.position - transform.position).normalized, Vector3.up);
                    if (Physics.Raycast(transform.position, (enemy.position - transform.position).normalized, out RaycastHit hit, attackRange))
                    {
                        if (hit.transform == enemy)
                        {
                            enemy.GetComponent<Health>().Hit(Random.Range(10, 25));
                            if (enemy.GetComponent<Health>().health <= 0)
                            {
                                enemy = null;
                                AcquireTarget(); // If the enemy died, get a new target
                            }
                        }
                    }
                }
            }
        }
        else
        {
            // If the NPC is dead, disable the NavMeshAgent and this script
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<Rigidbody>().useGravity = false;
            GetComponent<Collider>().enabled = false;
            this.enabled = false;
            agent.enabled = false;
        }
    }

    public void Attack(GameObject enemy)
    {
        this.enemy = enemy.transform;
        agent.SetDestination(this.enemy.position);
        agent.stoppingDistance = attackRange;
    }
    public void StopAttack()
    {
        enemy = null;
        AcquireTarget();
    }

    protected abstract void AcquireTarget();

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Gun") && !hasGun)
        {
            hasGun = true;
            other.gameObject.SetActive(false);
            other.transform.parent.SetParent(gunHolder, false);
            other.transform.parent.GetChild(0).localPosition = Vector3.zero;
            other.GetComponentInParent<Rotation>().enabled = false;
            AcquireTarget();
        }
    }
}