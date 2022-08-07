using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    public float RotationSpeed = 30;
    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(transform.position, transform.up, Time.deltaTime * RotationSpeed);
        transform.RotateAround(transform.position, transform.right, Time.deltaTime * RotationSpeed);
        transform.RotateAround(transform.position, transform.forward, Time.deltaTime * RotationSpeed);
    }
}
