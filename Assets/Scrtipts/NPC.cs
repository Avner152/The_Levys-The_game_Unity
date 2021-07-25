using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

// Base class for all NPCs
// It manages when the NPC would pick a new target, the attack and gun picking
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
        // Initialize the variables
        enemy = null; // The NPC doesn't have an enemy at the begining
        hasGun = false; // The NPC doesn't have a gun at the begining
        lastAttack = Time.time - attackDelay;

        // Find a target to go to (should be a gun)
        AcquireTarget();
    }

    // Update is called once per frame
    void Update()
    {
        // First check if the NPC is still alive
        if (health.GetCurrentHealth() > 0)
        {
            // If the NPC didn't get to the target, tell it to keep moving
            if (agent.remainingDistance > agent.stoppingDistance)
            {
                // Tell the character to move
                character.Move(agent.desiredVelocity);
            }
            else
            {
                // Stop the character if it reached the target
                character.Move(Vector3.zero);
                // If the NPC doesn't have an enemy, just get a new target
                if (enemy == null)
                {
                    AcquireTarget();
                }
                // If The NPC does have an enemy, attack it
                else
                {
                    // Look at the enemy (to make the aim more accurate)
                    transform.LookAt(enemy);
                    // Check if we can attack at this time point
                    if (Time.time > lastAttack + attackDelay)
                    {
                        lastAttack = Time.time;
                        // Play the muzzle flash effect
                        gunHolder.GetComponentInChildren<ParticleSystem>().Play();
                        // Fire a ray from our position towards the enemy, and check if it hit
                        if (Physics.Raycast(transform.position + Vector3.up * 1.5f, transform.forward, out RaycastHit hit, attackRange))
                        {
                            // If the ray hitted the enemy
                            if (hit.transform == enemy)
                            {
                                // Update its health
                                enemy.GetComponent<Health>().Hit(Random.Range(5, 30));
                                // Check if the enem died
                                if (enemy.GetComponent<Health>().GetCurrentHealth() <= 0)
                                {
                                    // If so, get a new target
                                    enemy = null;
                                    AcquireTarget();
                                }
                            }
                        }
                        else // If we can't hit the enemy then find a new target
                        {
                            enemy = null;
                            AcquireTarget();
                        }
                    }
                }
            }
        }
        else
        {
            // If the NPC is dead, disable all the relevant scripts (we can't disable the object because we want to see the NPC)
            this.enabled = false;
            GetComponentInChildren<NPCAttack>().enabled = false; // Make sure we won't detect anymore enemies
            GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Discrete;
            GetComponent<Rigidbody>().isKinematic = true; // Disable the physics on the character, so it won't fall when disabling the collider
            GetComponent<Rigidbody>().useGravity = false;
            GetComponent<Collider>().enabled = false; // Turn of the collider, because we don't want to hit a dead character
            gunHolder.gameObject.SetActive(false); // Hide the gun
            agent.enabled = false; // And disable AI navigation
        }
    }

    public void Attack(GameObject enemy)
    {
        // NPC only attack when it has a gun
        if (hasGun)
        {
            this.enemy = enemy.transform;
            agent.SetDestination(this.enemy.position);
            // Make sure to stop far enough and not too close
            agent.stoppingDistance = attackRange;
        }
    }
    public void StopAttack()
    {
        // To stop an attack, we just forget the enemy and find a new target
        enemy = null;
        AcquireTarget();
    }

    protected abstract void AcquireTarget();

    void OnTriggerEnter(Collider other)
    {
        // If the NPC touched a gun and it doesn't have a gun already
        if (other.gameObject.CompareTag("Gun") && !hasGun)
        {
            // Pick the gun
            hasGun = true;
            other.gameObject.SetActive(false);
            other.transform.parent.SetParent(gunHolder, false);
            other.transform.parent.GetChild(0).localPosition = Vector3.zero;
            other.GetComponentInParent<Rotation>().enabled = false; // Make sure to stop the rotation
            // After we picked the gun, find a qcquire a new target to go to
            AcquireTarget();
        }
    }
}