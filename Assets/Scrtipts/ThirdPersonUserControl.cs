using TMPro;
using UnityEngine;

namespace UnityStandardAssets.Characters.ThirdPerson
{
    [RequireComponent(typeof(ThirdPersonCharacter))]
    public class ThirdPersonUserControl : MonoBehaviour
    {
        public ThirdPersonCharacter character; // A reference to the ThirdPersonCharacter on the object
        public Transform gunHolder;
        public GameObject pickGunText;
        private Camera cam;                  // A reference to the main camera in the scene
        private Vector3 move;
        private float ry = 0;
        private bool hasGun = false;


        private void Start()
        {
            cam = Camera.main;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
            // read inputs
            float y = Input.GetAxis("Mouse Y");
            float h = Input.GetAxis("Mouse X");
            if (Mathf.Abs(y) >= 0.25)
            {
                h = 0;
            }
            float v = Input.GetAxis("Vertical");
            ry -= y;
            cam.transform.localEulerAngles = new Vector3(ry, 0, 0);
            move = new Vector3(h, 0, v);
            move = transform.TransformDirection(move);

            // Check if gun is in field of view
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider.gameObject.CompareTag("Gun"))
            {
                pickGunText.SetActive(true);
                pickGunText.GetComponent<TextMeshProUGUI>().SetText("Click the mouse to pick " + hit.collider.transform.parent.name);
                // If the left mouse buttons is clicked, pick the weapon
                if (Input.GetMouseButtonDown(0))
                {
                    hit.collider.gameObject.SetActive(false);
                    // Take only the gun model
                    hit.collider.transform.parent.SetParent(gunHolder, false);
                    hit.collider.transform.parent.GetChild(0).localPosition = Vector3.zero;
                    hit.collider.GetComponentInParent<Rotation>().enabled = false;
                    hasGun = true;
                }
            }
            else
            {
                pickGunText.SetActive(false);
            }

            if (hasGun && Input.GetKeyDown(KeyCode.Space))
            {
                if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.CompareTag("Character"))
                {
                    hit.collider.gameObject.SetActive(false);
                }
            }
        }

        // Fixed update is called in sync with physics
        private void FixedUpdate()
        {
            // pass all parameters to the character control script
            character.Move(move);
        }
    }
}
