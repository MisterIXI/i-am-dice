using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FollowCam : MonoBehaviour
{
    public Transform target;
    private Vector3 direction;
    private Vector2 directionChange;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = target.position + direction;
        transform.LookAt(target);
        direction = (new Vector3(directionChange.x, directionChange.y, 0) + direction).normalized * 10;
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
                Debug.Log("Direction change: " + directionChange);
            }
            else if (context.action.phase == InputActionPhase.Canceled)
            {
                directionChange = new Vector2(0, 0);
            }
        }
    }
}
