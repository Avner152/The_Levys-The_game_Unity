using UnityEngine;
using UnityEngine.UI;

// Keeps track of the health of a character
public class Health : MonoBehaviour
{
    public Animator anim;
    public int maxHealth = 100;
    public UpdatesViewer updatesViewer;
    public Image healthBar;
    private int currentHealth;

    private void Start()
    {
        // Initialize the current health
        currentHealth = maxHealth;
        // Update the healthbar graphics
        healthBar.fillAmount = currentHealth / maxHealth;
    }

    public void Hit(int damage)
    {
        currentHealth -= damage;
        updatesViewer.WriteUpdate("<color=#00FFFF>" + name + "</color> took <color=#FFA500>" + damage + "</color> damage points!\n");
        // If the character lost all its HP
        if (currentHealth <= 0)
        {
            // Clip the current health to 0 to prevent bugs in UI
            currentHealth = 0;
            // Start the dead animation
            anim.SetBool("Dead", true);
            updatesViewer.WriteUpdate("<color=#00FFFF>" + name + "</color> <color=red>died</color>!\n");
        }
        // Update the healthbar graphics
        healthBar.fillAmount = (float) currentHealth / maxHealth;
    }
    public void Heal(int heal)
    {
        currentHealth += heal;
        // If the character completly healed
        if (currentHealth > maxHealth)
        {
            // Clip the current health to max health to prevent bug in UI
            currentHealth = maxHealth;
        }
        // Update the healthbar graphics
        healthBar.fillAmount = (float)currentHealth / maxHealth;
        updatesViewer.WriteUpdate("<color=#00FFFF>" + name + "</color> regained <color=green>" + heal + "</color> HP\n");
    }
    public int GetCurrentHealth()
    {
        return currentHealth;
    }
    public bool IsHeart()
    {
        // The character is heart if the current health is less than the max health
        return currentHealth < maxHealth;
    }
}