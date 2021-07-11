using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody rb;
    public float speed = 10;
    public float angularSpeed = 2;
    public GameObject gameCamera;
    public GameObject text;
    private Vector3 movementDir;
    private float cameraRotation = 0;

    void Start()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }
        // Lock the cursor at the center and hide it
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        // Handle rotation
        float ry = Input.GetAxis("Mouse X") * angularSpeed;
        float rx = Input.GetAxis("Mouse Y") * angularSpeed;
        transform.Rotate(Vector3.up * ry);
        cameraRotation -= rx;
        //cameraRotation = Mathf.Clamp(cameraRotation, -75, 90);
        gameCamera.transform.localEulerAngles = new Vector3(cameraRotation, 0, 0);

        // Handle Movement
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        movementDir = new Vector3(h, 0, v).normalized;
        movementDir = transform.TransformDirection(movementDir);

        // Check if gun is in field of view
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit) && !hit.collider.name.Equals("Plane"))
        {
            text.SetActive(true);
            text.GetComponent<TextMeshProUGUI>().SetText("Press Space to pick " + hit.collider.name);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                hit.collider.gameObject.SetActive(false);
                hit.collider.transform.parent.SetParent(transform);
            }
        }
        else
        {
            text.SetActive(false);
        }
    }
    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movementDir * speed * Time.fixedDeltaTime);
    }
}
