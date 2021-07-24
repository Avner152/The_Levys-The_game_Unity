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
    public float attackDelay = 1f;
    protected bool hasGun;
    private Transform enemy;
    private float lastAttack;

    // Start is called before the first frame update
    void Start()
    {
        enemy = null;
        hasGun = false;
        AcquireTarget();
        lastAttack = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (health.GetCurrentHealth() > 0)
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
                    transform.LookAt(enemy);
                    if (Time.time > lastAttack + attackDelay)
                    {
                        lastAttack = Time.time;
                        gunHolder.GetComponentInChildren<ParticleSystem>().Play();
                        //if (Physics.BoxCast(transform.position + transform.forward * attackRange / 2, new Vector3(0.5f, 0.5f, 0.5f) * attackRange, transform.forward, out RaycastHit hit))
                        if (Physics.Raycast(transform.position + Vector3.up * 1.5f, transform.forward, out RaycastHit hit, attackRange))
                        {
                            if (hit.transform == enemy)
                            {
                                enemy.GetComponent<Health>().Hit(Random.Range(5, 30));
                                if (enemy.GetComponent<Health>().GetCurrentHealth() <= 0)
                                {
                                    enemy = null;
                                    AcquireTarget(); // If the enemy died, get a new target
                                }
                            }
                        }
                        else // If we can't hit the enemy, find a new target
                        {
                            enemy = null;
                            AcquireTarget(); // If the enemy died, get a new target
                        }
                    }
                }
            }
        }
        else
        {
            // If the NPC is dead, disable the NavMeshAgent and this script
            this.enabled = false;
            GetComponentInChildren<NPCAttack>().enabled = false;
            GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Discrete;
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<Rigidbody>().useGravity = false;
            GetComponent<Collider>().enabled = false;
            gunHolder.gameObject.SetActive(false);
            agent.enabled = false;
        }
    }

    public void Attack(GameObject enemy)
    {
        if (hasGun)
        {
            this.enemy = enemy.transform;
            agent.SetDestination(this.enemy.position);
            agent.stoppingDistance = attackRange;
        }
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