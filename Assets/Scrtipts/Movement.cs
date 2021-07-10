using UnityEngine;

public class Movement : MonoBehaviour
{
    public GameObject gameCamera;
    private CharacterController characterController;
    private float speed = 4f, rx = 0, ry = 0;
    private float angularSpeed = 1f;
    public Rigidbody rb;
    private Vector3 motion;

    // Start is called before the first frame update
    private void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        //characterController = this.GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnDestroy()
    {
        Cursor.lockState = CursorLockMode.None;
    }
    
    // Update is called once per frame
    private void Update()
    {
        float dx, dz;
        // mouse
        rx -= Input.GetAxis("Mouse Y") * angularSpeed;
        ry += Input.GetAxis("Mouse X") * angularSpeed;
  
        gameCamera.transform.localEulerAngles = new Vector3(rx, 0, 0);
        transform.localEulerAngles = new Vector3(0, ry, 0);

        dx = Input.GetAxis("Horizontal") * speed;
        dz = Input.GetAxis("Vertical") * speed ;

        motion = new Vector3(dx, 0, dz);
        motion = transform.TransformDirection(motion);
        rb.MovePosition(rb.position + motion);

        //characterController.Move(motion);
      // transform.Translate(motion);

    }
}
