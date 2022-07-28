using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounciness : MonoBehaviour
{
    public int BouncyFactor = 5;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.rigidbody)
        {
            collision.rigidbody.velocity = transform.TransformDirection(new Vector3(0, Mathf.Abs(collision.relativeVelocity.y) + BouncyFactor, 0));
        }
    }
}
