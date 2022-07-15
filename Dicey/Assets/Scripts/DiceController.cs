using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class DiceController : MonoBehaviour
{
    Vector2 Movement;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        Movement = new Vector2(0, 0);
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        if (Movement.x != 0 || Movement.y != 0)
        {
            rb.AddForce(Movement * 10);
        }
    }

    public void MoveDice(InputAction.CallbackContext context)
    {
        if (context.action.name == "MoveDice")
        {
            if (context.action.phase == InputActionPhase.Started)
            {
                Debug.Log("Started");
            }
            else if (context.action.phase == InputActionPhase.Performed)
            {
                Debug.Log("Performed");
            }
            else if (context.action.phase == InputActionPhase.Canceled)
            {
                Debug.Log("Canceled");
            }
        }
    }

    // gizmos
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        // draw force arrow
        Gizmos.DrawRay(transform.position, new Vector3(1,1.5f,0) * 10);
    }
}
