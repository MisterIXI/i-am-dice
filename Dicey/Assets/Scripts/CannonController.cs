using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CannonController : MonoBehaviour
{
    public float ShootingForce = 6000f;
    public ParticleSystem SmokeParticles;

    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            SmokeParticles.Play();
            _audioSource.Play();
            other.attachedRigidbody.AddForce(ShootingForce * transform.forward);
        }
    }
}
