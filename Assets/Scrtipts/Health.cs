using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public Animator anim;
    public int maxHealth = 100;
    public UpdatesViewer updatesViewer;
    public Image healthBar;
    private int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
        healthBar.fillAmount = currentHealth / maxHealth;
    }

    public void Hit(int damage)
    {
        currentHealth -= damage;
        updatesViewer.WriteUpdate("<color=#00FFFF>" + name + "</color> took <color=#FFA500>" + damage + "</color> damage points!\n");
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            anim.SetBool("Dead", true);
            updatesViewer.WriteUpdate("<color=#00FFFF>" + name + "</color> <color=red>died</color>!\n");
        }
        healthBar.fillAmount = (float) currentHealth / maxHealth;
    }
    public void Heal(int heal)
    {
        currentHealth += heal;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        healthBar.fillAmount = (float)currentHealth / maxHealth;
        updatesViewer.WriteUpdate("<color=#00FFFF>" + name + "</color> regained <color=green>" + heal + "</color> HP\n");
    }
    public int GetCurrentHealth()
    {
        return currentHealth;
    }
    public bool IsHeart()
    {
        return currentHealth < maxHealth;
    }
}