using TMPro;
using UnityEngine;

public class Health : MonoBehaviour
{
    public Animator anim;
    public int health = 100;
    public TextMeshProUGUI updatesText;

    public void Hit(int damage)
    {
        health -= damage;
        updatesText.SetText(updatesText.text + "\n" + name + " took " + damage + " damage points!\n");
        if (health <= 0)
        {
            health = 0;
            anim.SetBool("Dead", true);
            updatesText.SetText(updatesText.text + "\n" + name + " died!\n");
        }
    }
}