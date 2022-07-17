using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : MonoBehaviour
{
    public float ShootingForce = 6000f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.attachedRigidbody.AddForce(ShootingForce * transform.forward);
        }
    }
}