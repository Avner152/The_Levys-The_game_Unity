using UnityEngine;

public class Rotation : MonoBehaviour
{
    public float angularSpeed = 2;

    void Update()
    {
        transform.Rotate(Vector3.up * angularSpeed * Time.deltaTime);
    }
}
