using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FollowCam : MonoBehaviour
{
    public Transform target;
    public float speed = 5f;
    private Vector3 direction;
    private Vector2 directionChange;
    // Start is called before the first frame update
    void Start()
    {
        direction = new Vector3(5, 0, 5);
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        float oldX = transform.rotation.y;
        transform.position = target.position + direction;
        transform.LookAt(target);
        transform.RotateAround(target.position, Vector3.up, directionChange.x * speed);
        float angle = Vector3.Angle(new Vector3(direction.x, 0, direction.z), direction);

        if (direction.y > 0)
        {
            if (angle + directionChange.y * speed < 90f)
                transform.RotateAround(target.position, transform.right, directionChange.y * speed);
        }
        else
        {
            if (angle - directionChange.y * speed < 90f)
                transform.RotateAround(target.position, transform.right, directionChange.y * speed);
        }

        direction = transform.position - target.position;
    }

    public void RotateCamera(InputAction.CallbackContext context)
    {
        if (context.action.name == "RotateCamera")
        {
            if (context.action.phase == InputActionPhase.Started)
            {
                // Debug.Log("Started");
            }
            else if (context.action.phase == InputActionPhase.Performed)
            {
                directionChange = context.action.ReadValue<Vector2>();
                // Debug.Log("Direction change: " + directionChange);
            }
            else if (context.action.phase == InputActionPhase.Canceled)
            {
                directionChange = new Vector2(0, 0);
            }
        }
    }
}
