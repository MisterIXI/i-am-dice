using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : MonoBehaviour
{
    public float ShootingForce = 6000f;
    public ParticleSystem SmokeParticles;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player collided with cannon trigger.");
            SmokeParticles.Play();
            other.attachedRigidbody.AddForce(ShootingForce * transform.forward);
        }
    }
}
