using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ForcePush : MonoBehaviour
{
    public bool ShootFromCenter;
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

            if(ShootFromCenter)
                other.transform.position = transform.position;
            other.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            if(_audioSource)
                _audioSource.Play();
            if(SmokeParticles)
                SmokeParticles.Play();
            other.attachedRigidbody.AddForce(ShootingForce * transform.forward);
        }
    }
}
