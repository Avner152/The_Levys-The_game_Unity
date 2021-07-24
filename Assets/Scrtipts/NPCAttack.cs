using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAttack : MonoBehaviour
{
    public Teams teams;
    public NPC npc;
    private GameObject currentEnemy;

    // Start is called before the first frame update
    void Start()
    {
        currentEnemy = null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Character") && !teams.InSameTeam(other.gameObject, transform.parent.gameObject))
        {
            currentEnemy = other.gameObject;
            npc.Attack(currentEnemy);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == currentEnemy)
        {
            npc.StopAttack();
            currentEnemy = null;
        }
    }
    void OnDisable()
    {
        GetComponent<Collider>().enabled = false;
    }
}
