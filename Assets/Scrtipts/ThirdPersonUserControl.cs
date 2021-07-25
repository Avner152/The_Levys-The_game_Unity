using TMPro;
using UnityEngine;

namespace UnityStandardAssets.Characters.ThirdPerson
{
    // The class handles the player movement, attack and weapon picking
    [RequireComponent(typeof(ThirdPersonCharacter))]
    public class ThirdPersonUserControl : MonoBehaviour
    {
        public ThirdPersonCharacter character; // A reference to the ThirdPersonCharacter on the object
        public Transform gunHolder;
        public TextMeshProUGUI pickGunText;
        public GameObject grenadePrefab;
        public float rotationSensetivity = 0.5f;
        public float throwForce = 40f;
        public float attackDelay = 0.5f;
        private Camera cam;                  // A reference to the main camera in the scene
        private Vector3 move;
        private float ry = 0;
        private bool hasGun = false;
        private bool hasGrenade = false;
        private Health health;
        private float lastAttack;

        private void Start()
        {
            cam = Camera.main; // Get reference to the main camera
            health = GetComponent<Health>(); // Get reference to the health script
            Cursor.lockState = CursorLockMode.Locked; // Lock th cursor at the center of the screen
        }

        private void Update()
        {
            // If the player is still alive
            if (health.GetCurrentHealth() > 0)
            {
                // read inputs
                float y = Input.GetAxis("Mouse Y");
                float h = Input.GetAxis("Mouse X") * rotationSensetivity;
                // To make the controls easier to use, cancel the horizontal input if we have vertical input (can rotate diagonally)
                if (Mathf.Abs(y) >= 0.4)
                {
                    h = 0;
                }
                float v = Input.GetAxis("Vertical");
                // Update the camera rotaion
                ry -= y;
                cam.transform.localEulerAngles = new Vector3(ry, 0, 0);
                // Save the movement vector for this frame
                move = new Vector3(h, 0, v);
                move = transform.TransformDirection(move);

                // Check if gun is in field of view
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                // Check if the player looks at a gun, if so make sure it is not too far
                if (Physics.Raycast(ray, out RaycastHit hit) && hit.distance <= 1.5 && (hit.collider.gameObject.CompareTag("Gun") || hit.collider.gameObject.CompareTag("Grenade")))
                {
                    pickGunText.gameObject.SetActive(true);
                    pickGunText.SetText("Click the mouse to pick " + hit.collider.transform.parent.name);
                    // If the left mouse buttons is clicked, pick the weapon
                    if (Input.GetMouseButtonDown(0))
                    {
                        // If we click on a gun
                        if (hit.collider.gameObject.CompareTag("Gun"))
                        {
                            if (hasGun)
                            {
                                // If we have a gun then destroy it (user can't have 2 guns)
                                Destroy(gunHolder.GetChild(0).gameObject);
                            }
                            hit.collider.gameObject.SetActive(false);
                            hit.collider.transform.parent.SetParent(gunHolder, false);
                            hit.collider.transform.parent.GetChild(0).localPosition = Vector3.zero;
                            hit.collider.GetComponentInParent<Rotation>().enabled = false;
                            hasGun = true;
                        }
                        // If we click on a grenades box
                        else if (hit.collider.gameObject.CompareTag("Grenade"))
                        {
                            hit.transform.parent.gameObject.SetActive(false);
                            hasGrenade = true;
                        }
                    }
                }
                else
                {
                    pickGunText.gameObject.SetActive(false);
                }

                // If we have a gun and trying to fire (clicking the Space key) and the attack delay passed
                if (hasGun && Input.GetKey(KeyCode.Space) && Time.time > lastAttack + attackDelay)
                {
                    lastAttack = Time.time;
                    // Play the muzzle flash effects
                    gunHolder.GetComponentInChildren<ParticleSystem>().Play();
                    // If we hitted a character
                    if (Physics.Raycast(ray, out RaycastHit hit1) && hit1.collider.gameObject.CompareTag("Character"))
                    {
                        // Hit the character with random damage between 10 to 25
                        hit1.collider.GetComponent<Health>().Hit(Random.Range(10, 25));
                    }
                }
                // If we have a grenade and trying to throw it (clicking the Q key)
                if (hasGrenade && Input.GetKeyDown(KeyCode.Q))
                {
                    ThrowGrenade();
                }
            }
            else
            {
                // If the player is dead, disable all relevant components
                GetComponent<Rigidbody>().isKinematic = true;
                GetComponent<Rigidbody>().useGravity = false;
                GetComponent<Collider>().enabled = false;
                this.enabled = false;
                character.enabled = false;
            }

        }

        private void ThrowGrenade()
        {
            // Create an instance of the grenade prefab
            GameObject grenade = Instantiate(grenadePrefab, cam.transform.position, cam.transform.rotation);
            // Get reference to the Rigidbody
            Rigidbody rb = grenade.GetComponent<Rigidbody>();
            // Apply the throw force in the direction of the camera (the direction the player looks at)
            rb.AddForce(cam.transform.forward * throwForce, ForceMode.VelocityChange);
        }

        // Fixed update is called in sync with physics
        private void FixedUpdate()
        {
            // pass all parameters to the character control script
            character.Move(move);
        }

        private void OnTriggerEnter(Collider other)
        {
            // If we touch a health bonus and we got heart
            if (other.gameObject.CompareTag("Health") && health.IsHeart())
            {
                // Random heal amount between 15 to 35
                int healAmount = Random.Range(15, 35);
                // Apply the healing
                health.Heal(healAmount);
                // Destroy the Health bonus, so we won't use it again
                Destroy(other.transform.parent.gameObject);
            }
        }
    }
}
