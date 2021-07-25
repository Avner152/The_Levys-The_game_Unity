using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Simple class to detect if an enemy is nearby
public class NPCAttack : MonoBehaviour
{
    public Teams teams;
    public NPC npc;
    private GameObject currentEnemy;

    // Start is called before the first frame update
    void Start()
    {
        currentEnemy = null; // We don't have an enemy at the begining
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider other)
    {
        // Check if a collider entered the detection area, and it is a character from the other team (an enemy)
        if (other.gameObject.CompareTag("Character") && !teams.InSameTeam(other.gameObject, transform.parent.gameObject))
        {
            // Save the reference to it and attack
            currentEnemy = other.gameObject;
            npc.Attack(currentEnemy);
        }
    }
    void OnTriggerExit(Collider other)
    {
        // If the current enemy leaved the attack zone, stop the attack
        if (other.gameObject == currentEnemy)
        {
            npc.StopAttack();
            currentEnemy = null;
        }
    }
    void OnDisable()
    {
        // Turn off the collider when this script is disabled, so we won't detect any more enemies
        GetComponent<Collider>().enabled = false;
    }
}
