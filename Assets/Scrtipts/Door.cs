using UnityEngine;

// Simple class to handle the door animation everytime a collider enters its area
public class Door : MonoBehaviour
{
    public Animator anim;
    private int counter;

    private void OnTriggerEnter(Collider other)
    {
        // Count how many coliiders entered the door area
        counter++;
        anim.SetBool("IsOpen", true);
    }
    private void OnTriggerExit(Collider other)
    {
        counter--;
        // Close the door only if there is no one in the door area
        if (counter == 0)
        {
            anim.SetBool("IsOpen", false);
        }
    }
}
