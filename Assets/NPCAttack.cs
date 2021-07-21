using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAttack : MonoBehaviour
{
    public Teams teams;
    public NPC npc;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Character") && !teams.InSameTeam(other.gameObject, transform.parent.gameObject))
        {
            print("Enemy detected");
            npc.Attack(other.gameObject);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Character") && !teams.InSameTeam(other.gameObject, transform.parent.gameObject))
        {
            print("Enemy detected");
            npc.StopAttack();
        }
    }
}
