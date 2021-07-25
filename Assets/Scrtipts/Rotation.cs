using UnityEngine;

// A class that rotates the object's transform in a given speed
public class Rotation : MonoBehaviour
{
    public float angularSpeed = 2;

    void Update()
    {
        // Rotate the transform
        transform.Rotate(Vector3.up * angularSpeed * Time.deltaTime);
    }
    void OnDisable()
    {
        // Reset the rotation when this script is disabled
        transform.localEulerAngles = Vector3.zero;
    }
}
