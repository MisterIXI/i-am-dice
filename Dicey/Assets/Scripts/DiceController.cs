using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.InputSystem;
public class DiceController : MonoBehaviour
{
    private float JUMP_COOLDOWN = 1f;

    private Vector2 _movement;
    [HideInInspector]
    public Vector2 CurrentInput;
    Rigidbody _rb;
    public float JumpStrength = 800;
    private bool _isJumping = false;
    private bool _isOnJumpCooldown = false;
    private Vector3 _initialPosition;
    private Quaternion _initialRotation;
    private Material _material;
    private Material _material2;

    void Start()
    {
        _movement = new Vector2(0, 0);
        _rb = GetComponent<Rigidbody>();
        CurrentInput = new Vector2(0, 0);
        _initialPosition = transform.position;
        _initialRotation = transform.rotation;
        _material = GetComponent<MeshRenderer>().material;
        _material2 = GetComponent<MeshRenderer>().materials[1];
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        if (IsMoving())
        {
            _rb.AddForce(GetMovementVector());
        }
        CheckForFloor();
    }

    private bool IsMoving()
    {
        return _movement.x != 0 || _movement.y != 0;
    }
    private Vector3 GetMovementVector()
    {
        return new Vector3(_movement.x, 0, _movement.y) * 10;
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
                updateMovement(context.action.ReadValue<Vector2>());
                CurrentInput = context.action.ReadValue<Vector2>();
            }
            else if (context.action.phase == InputActionPhase.Canceled)
            {
                _movement = new Vector2(0, 0);
                CurrentInput = new Vector2(0, 0);
            }
        }
    }

    public void updateMovement(Vector2 currentMovement)
    {
        // adjust Vector2 to match the camera angle
        Vector2 adjustedMovement = currentMovement;
        Vector2 viewDir = new Vector2(Camera.main.transform.forward.x, Camera.main.transform.forward.z);
        // -1.5708f is the constant angle of the calculation
        float angle = Mathf.Atan2(viewDir.y, viewDir.x) - 1.5708f;
        adjustedMovement = new Vector2(
            adjustedMovement.x * Mathf.Cos(angle) - adjustedMovement.y * Mathf.Sin(angle),
            adjustedMovement.x * Mathf.Sin(angle) + adjustedMovement.y * Mathf.Cos(angle));
        _movement = adjustedMovement;
        Debug.Log("Movement: " + _movement + " currentMovement: " + currentMovement);
    }
    public void Jump(InputAction.CallbackContext context)
    {
        if (context.action.name == "Jump")
        {
            if (!_isJumping && !_isOnJumpCooldown)
            {

                if (context.action.phase == InputActionPhase.Performed)
                {
                    _rb.AddForce(new Vector3(0, JumpStrength, 0));
                    _rb.AddTorque(new Vector3(Random.Range(1f, 2f) * JumpStrength, Random.Range(1f, 2f) * JumpStrength, Random.Range(1f, 2f) * JumpStrength));
                    StartCoroutine(JumpCooldown());
                }
                else if (context.action.phase == InputActionPhase.Canceled)
                {
                    // Debug.Log("Canceled");
                }
            }
        }
    }

    private string _debugString = "";
    private void CheckForFloor()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 2.5f))
        {
            _debugString = "Hit: " + hit.collider.gameObject.name;
            if (hit.collider.gameObject.tag != "DiceFace")
            {
                _isJumping = false;
                _material2.color = Color.green;
            }
            // else
            // {
            //     _isJumping = true;
            //     _material2.color = Color.red;
            // }
        }
        else
        {
            _isJumping = true;
            _material2.color = Color.red;
        }
    }
    private IEnumerator JumpCooldown()
    {
        _isOnJumpCooldown = true;
        //darken material color
        Color oldColor = _material.color;
        _material.color = new Color(_material.color.r / 3, _material.color.g / 3, _material.color.b / 3, _material.color.a);
        Vector3 colorSteps = new Vector3(_material.color.r / 10, _material.color.g / 10, _material.color.b / 10);
        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(JUMP_COOLDOWN / 10);
            _material.color = new Color(_material.color.r + colorSteps.x, _material.color.g + colorSteps.y, _material.color.b + colorSteps.z, _material.color.a);
        }
        _isOnJumpCooldown = false;
        _material.color = oldColor;
    }

    public void ResetPosition(InputAction.CallbackContext context)
    {
        if (context.action.name == "ResetPosition")
        {
            if (context.action.phase == InputActionPhase.Performed)
            {
                transform.position = _initialPosition;
                transform.rotation = _initialRotation;
                _rb.velocity = Vector3.zero;
                _rb.angularVelocity = Vector3.zero;
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

        Gizmos.DrawRay(transform.position, Vector3.down * 3f);
        Handles.Label(transform.position, _debugString);
        if (IsMoving())
        {
            Gizmos.DrawRay(transform.position, GetMovementVector());
        }
    }
}
