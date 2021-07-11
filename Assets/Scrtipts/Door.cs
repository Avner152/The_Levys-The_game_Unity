using UnityEngine;

public class Door : MonoBehaviour
{
    public Collider coll;
    public Animator anim;
    private int counter;

    private void OnTriggerEnter(Collider other)
    {
        counter++;
        anim.SetBool("IsOpen", true);
    }
    private void OnTriggerExit(Collider other)
    {
        counter--;
        // Close the door only if there is no one in its range
        if (counter == 0)
        {
            anim.SetBool("IsOpen", false);
        }
    }
}
