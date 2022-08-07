using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    public float DoorRotation;
    public float RotationSpeed;
    public Rigidbody rb;
    public void OpenDoor()
    {
        StartCoroutine(RotateDoor());
    }


    public IEnumerator RotateDoor()
    {
        float timeElapsed = 0;
        Vector3 currentRotation = transform.rotation.eulerAngles;
        while (timeElapsed < RotationSpeed)
        {
            float t = timeElapsed / RotationSpeed;
            t = t * t * (3f - 2f * t);
            Vector3 finalRotation = new Vector3(currentRotation.x, currentRotation.y + DoorRotation, currentRotation.z);
            Vector3 currentRot = Vector3.Lerp(currentRotation, finalRotation, t);
            rb.rotation = Quaternion.Euler(currentRot);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }
}
