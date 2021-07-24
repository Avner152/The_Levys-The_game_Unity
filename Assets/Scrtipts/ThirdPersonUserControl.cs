using TMPro;
using UnityEngine;

namespace UnityStandardAssets.Characters.ThirdPerson
{
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
            cam = Camera.main;
            health = GetComponent<Health>();
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
            if (health.GetCurrentHealth() > 0)
            {
                // read inputs
                float y = Input.GetAxis("Mouse Y");
                float h = Input.GetAxis("Mouse X") * rotationSensetivity;
                if (Mathf.Abs(y) >= 0.4)
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
                // Check if the player looks at a gun, if so make sure it is not too far
                if (Physics.Raycast(ray, out RaycastHit hit) && hit.distance <= 1.5 && (hit.collider.gameObject.CompareTag("Gun") || hit.collider.gameObject.CompareTag("Grenade")))
                {
                    pickGunText.gameObject.SetActive(true);
                    pickGunText.SetText("Click the mouse to pick " + hit.collider.transform.parent.name);
                    // If the left mouse buttons is clicked, pick the weapon
                    if (Input.GetMouseButtonDown(0))
                    {
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

                if (hasGun && Input.GetKey(KeyCode.Space) && Time.time > lastAttack + attackDelay)
                {
                    lastAttack = Time.time;
                    gunHolder.GetComponentInChildren<ParticleSystem>().Play();
                    if (Physics.Raycast(ray, out RaycastHit hit1) && hit1.collider.gameObject.CompareTag("Character"))
                    {
                        // Hit the character with random damage between 10 to 25
                        hit1.collider.GetComponent<Health>().Hit(Random.Range(10, 25));
                    }
                }
                if (hasGrenade && Input.GetKeyDown(KeyCode.Q))
                {
                    ThrowGrenade();
                }
            }
            else
            {
                // If the character is dead, disable all relevant components
                GetComponent<Rigidbody>().isKinematic = true;
                GetComponent<Rigidbody>().useGravity = false;
                GetComponent<Collider>().enabled = false;
                this.enabled = false;
                character.enabled = false;
            }

        }

        private void ThrowGrenade()
        {
            GameObject grenade = Instantiate(grenadePrefab, cam.transform.position, cam.transform.rotation);
            Rigidbody rb = grenade.GetComponent<Rigidbody>();
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
            if (other.gameObject.CompareTag("Health") && health.IsHeart())
            {
                int healAmount = Random.Range(10, 35);
                health.Heal(healAmount);
                Destroy(other.transform.parent.gameObject);
            }
        }
    }
}
