using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public float delay = 3f;
    public float radius = 5f;
    public float force = 700;

    public AudioClip explosionAudio;
    public GameObject explosionEffect;

    float countDown;
    bool hasExploded = false;

    // Start is called before the first frame update
    void Start()
    {
        countDown = delay;
    }

    // Update is called once per frame
    void Update()
    {
        countDown -= Time.deltaTime;

        if (countDown <= 0 && !hasExploded)
        {
            Explode();
            hasExploded = true;
        }
    }

    private void Explode()
    {
        GameObject fx = Instantiate(explosionEffect, transform.position, transform.rotation);
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        AudioSource fxAudio = fx.AddComponent<AudioSource>();
        fxAudio.clip = explosionAudio;
        fxAudio.spatialBlend = 1;
        fxAudio.Play();

        foreach (Collider nearbyObject in colliders)
        {
            if (nearbyObject.TryGetComponent<Rigidbody>(out Rigidbody rigidbody))
            {
                rigidbody.AddExplosionForce(force, transform.position, radius);
            }
            if (nearbyObject.TryGetComponent<Health>(out Health characterHealth))
            {
                characterHealth.Hit(Mathf.RoundToInt(Mathf.Lerp(70, 10, Vector3.Distance(transform.position, nearbyObject.transform.position) / radius)));
            }
        }
        Destroy(gameObject);
    }
}
