using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CannonController : MonoBehaviour
{
    public float ShootingForce = 6000f;
    public ParticleSystem SmokeParticles;

    private AudioSource _audioSource;

    private void Start()
    {
        if(GetComponent<AudioSource>())
            _audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {

            other.transform.position = transform.position;
            other.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            SmokeParticles.Play();
            if(_audioSource)
                _audioSource.Play();
            other.attachedRigidbody.AddForce(ShootingForce * transform.forward);
        }
    }
}
