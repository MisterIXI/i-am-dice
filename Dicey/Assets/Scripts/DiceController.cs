using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
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
                // Debug.Log("Started");
            }
            else if (context.action.phase == InputActionPhase.Performed)
            {
                // adjust Vector2 to match the camera angle
                Vector2 adjustedMovement = context.action.ReadValue<Vector2>();
                Vector2 viewDir = new Vector2(Camera.main.transform.forward.x, Camera.main.transform.forward.z);
                float angle = Mathf.Atan2(viewDir.y, viewDir.x) - 1.5708f;
                // angle = angle<0? a
                adjustedMovement = new Vector2(
                    adjustedMovement.x * Mathf.Cos(angle) - adjustedMovement.y * Mathf.Sin(angle),
                    adjustedMovement.x * Mathf.Sin(angle) + adjustedMovement.y * Mathf.Cos(angle));
                Movement = adjustedMovement;
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
                // Debug.Log("Started");
            }
            else if (context.action.phase == InputActionPhase.Performed)
            {
                rb.AddForce(new Vector3(0, jumpStrength, 0));
                rb.AddTorque(new Vector3(Random.Range(1f, 2f) * jumpStrength, Random.Range(1f, 2f) * jumpStrength, Random.Range(1f, 2f) * jumpStrength));

            }
            else if (context.action.phase == InputActionPhase.Canceled)
            {
                // Debug.Log("Canceled");
            }
        }
    }

    // gizmos
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        // draw force arrow
        // Vector2 viewDir = new Vector2(Camera.main.transform.forward.x, Camera.main.transform.forward.z).normalized;
        // float angle = Mathf.Atan2(viewDir.y, viewDir.x) *Mathf.Rad2Deg;//Vector2.Angle(new Vector2(0, 1), viewDir);
        // if(angle < 0){
        //     GUI.color = Color.red;
        //     angle = 360 + angle;
        // }
        // else{
        //     GUI.color = Color.green;
        // }
        // Handles.Label(transform.position, "Angle: " + angle);

        if (IsMoving())
        {
            Gizmos.DrawRay(transform.position, GetMovementVector());
        }
    }
}
