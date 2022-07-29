using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollision : MonoBehaviour
{
    public Vector3 CamOffset = Vector3.zero;
    public Transform PlayerTarget;
    public LayerMask IgnoreCollision;
    public SphereCollider CameraCollider;
    bool _isCameraColliding;


    // Update is called once per frame
    void Update()
    {
        if (_isCameraColliding || IsCameraColliding())
        {
            ZoomToPlayer();
        }
        else if(IsCameraColliding())
        {
            ZoomToPlayer();
        }
    }



    private bool IsCameraColliding()
    {
        RaycastHit hit;
        Debug.DrawRay(transform.position, PlayerTarget.position - transform.position, Color.red);
        //check for collision between camera and player
        return (Physics.Raycast(transform.position, (PlayerTarget.position - transform.position), out hit, IgnoreCollision) && hit.collider.gameObject.layer == 8 );
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.layer == 8)
            _isCameraColliding = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 8)
            _isCameraColliding = false;
    }


    public float ZoomSpeed = 1;

    void ZoomToPlayer()
    {
        Debug.Log("Zoom to player");
        transform.position = Vector3.Lerp(transform.position, PlayerTarget.position, ZoomSpeed * Time.deltaTime);
    }

}
