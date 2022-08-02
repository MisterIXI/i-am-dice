using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ChallengeFallOver : MonoBehaviour
{
    [Tooltip("The angle between forward vector and ground below which the object is considered 'fallen over'.")]
    public float AngleThreshold = 30f;

    [Tooltip("The velocity below which the object must slow down to be considered fully 'fallen over'.")]
    public float VelocityThreshold = 0.5f;

    [Tooltip("The number of frames between two checks for being fallen over.")]
    public int UpdateFrequency = 10;

    public Vector3 DotSpawnOffset = new Vector3(0, 5, 0);

    private Rigidbody _rigidbody;
    private int _updateCount = 0;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if(++_updateCount < UpdateFrequency)
        {
            return;
        }

        _updateCount = 0;

        var velocity = _rigidbody.velocity.magnitude;
        if(velocity > VelocityThreshold) // don't spawn dot while object is still moving fast (e.g. flying around)
        {
            return;
        }

        var angleToGround = Vector3.Angle(transform.forward, new Vector3(transform.forward.x, 0, transform.forward.z));
        if(angleToGround < AngleThreshold)
        {
            SpawnReward();
            enabled = false;
        }
    }

    void SpawnReward()
    {
        DotAnimation.SpawnDot(transform.position + DotSpawnOffset);
    }
}
