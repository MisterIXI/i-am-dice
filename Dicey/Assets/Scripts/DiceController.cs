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
        if (IsMoving())
        {
            rb.AddForce(GetMovementVector());
        }
    }

    private bool IsMoving()
    {
        return Movement.x != 0 || Movement.y != 0;
    }
    private Vector3 GetMovementVector()
    {
        return new Vector3(Movement.x, 0, Movement.y) * 10;
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
                Movement = context.action.ReadValue<Vector2>();
            }
            else if (context.action.phase == InputActionPhase.Canceled)
            {
                Movement = new Vector2(0, 0);
            }
        }
    }
    public float jumpStrength = 800;
    public void Jump(InputAction.CallbackContext context)
    {
        if (context.action.name == "Jump")
        {
            if (context.action.phase == InputActionPhase.Started)
            {
                Debug.Log("Started");
            }
            else if (context.action.phase == InputActionPhase.Performed)
            {
                rb.AddForce(new Vector3(0, jumpStrength, 0));
                rb.AddTorque(new Vector3(Random.Range(1f, 2f) * jumpStrength, Random.Range(1f, 2f) * jumpStrength, Random.Range(1f, 2f) * jumpStrength));

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
        if (IsMoving())
        {
            Gizmos.DrawRay(transform.position, GetMovementVector());
        }
    }
}
